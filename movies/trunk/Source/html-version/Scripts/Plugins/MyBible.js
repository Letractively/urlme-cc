var column = ""; // this will get set in either Salem.HistoryManager.Callback, or in Salem.HistoryManager.Load, depending on the entry points

Salem.RegisterNamespace("MyBible");
Salem.MyBible = {
    SwitchView: function(view) {
        var idPrefix = "";
        switch (view) {
            case "RightArticle":
                idPrefix = "#RightColumn ";
                $(idPrefix + ".View").hide();
                $(idPrefix + "#TabbedView").show();
                Salem.MyBible.Navigation.UI.ParallelViewClicked();
                $(idPrefix + "#ParallelViewTab .ParallelView").hide();
                $(idPrefix + "#ParallelViewTab #ArticleView").show();
                Salem.MyBible.Search.UI.CollapseSearchBox('Right'); // juuuuuuuuuuuuust in case
                break;
            case "RightPassage":
                idPrefix = "#RightColumn ";
                $(idPrefix + ".View").hide();
                $(idPrefix + "#TabbedView").show();
                Salem.MyBible.Navigation.UI.ParallelViewClicked();
                $(idPrefix + "#ParallelViewTab .ParallelView").hide();
                $(idPrefix + "#ParallelViewTab .Passage-View").show();
                Salem.MyBible.Search.UI.CollapseSearchBox('Right');
                break;
            case "LeftPassage":
                idPrefix = "#LeftColumn ";
                $(idPrefix + ".View").hide();
                $(idPrefix + ".Passage-View").show();
                Salem.MyBible.Search.UI.CollapseSearchBox('Left');
                break;
            case "Default":
                Salem.MyBible.Navigation.UI.SetDefaultRightPaneView(); // defined in MyBible-Navigation.js
                idPrefix = "#LeftColumn ";
                $(idPrefix + ".View").hide();
                $(idPrefix + ".Passage-View").show();
                Salem.MyBible.Search.UI.CollapseSearchBox('Left');
                Salem.MyBible.Search.UI.CollapseSearchBox('Right');
                break;
            default:
                // do nothing. no way.
        }
    }
    , HiddenVerseSelections: function(value) {
        if (typeof value != 'undefined') {
            // setter for hidden input. updates every time a user selects a verse / multiple verses
            $("#HiddenVerseSelections").val(value);
        }
        else {
            // getter
            return $("#HiddenVerseSelections").val();
        }
    }
};

Salem.RegisterNamespace("HistoryManager");
Salem.HistoryManager = {
    Init: function () {
        $.history.init(Salem.HistoryManager.Callback);
    },
    LoadNewTranslation: function (newTranslation, leftOrRight) {
        // translation dropdown change event calls this function
        // scenarios:
        // 1. get to this point via /my-bible/, with no hash to work with for swapping out translation codes in url
        // 2. get to this point via /my-bible/existing hash, where we can swap out the translation code and push left or right w/ new hash
        if (leftOrRight == "Left") {
            if (Salem.String.IsNullOrEmpty(location.hash)) {
                Salem.HistoryManager.LoadLeft(Salem.String.Format('#/passage/{0}/genesis/1/', newTranslation));
            } else {
                var currentLeftHash = location.hash.split("&")[0].replace("#/", "");
                // example currentLeftHash = left:passage/niv/john/3/
                var existingTranslation = currentLeftHash.split("/")[1];
                var newHash = currentLeftHash.replace(existingTranslation, newTranslation);
                newHash = "#/" + newHash.replace("left:", "");
                Salem.HistoryManager.LoadLeft(newHash);
            }
        } else { // must be right
            if (Salem.String.IsNullOrEmpty(location.hash)) {
                Salem.HistoryManager.LoadRight(Salem.String.Format('#/passage/{0}/genesis/1/', newTranslation));
            } else {
                var currentRightHash = location.hash.split("&")[1];
                // example currentRightHash = right:passage/niv/john/3/
                var existingTranslation = currentRightHash.split("/")[1];
                var newHash = currentRightHash.replace(existingTranslation, newTranslation);
                newHash = "#/" + newHash.replace("right:", "");
                Salem.HistoryManager.LoadRight(newHash);
            }
        }
    },
    // LoadCmsArticlePage loads a passed-in page number for the current article
    LoadCmsArticlePage: function (page) {
        var currentHash = location.hash;
        var currentRightHash = currentHash.split("&")[1]; // right:cms/bible-study/3243.html/3 (page 3) OR right:cms/bible-study/3243.html (page 1)
        var nextRightHash = "";
        var currentLeftHash = currentHash.split("&")[0].replace("#/", "");
        var nextHash = "";

        if (Salem.String.IsNullOrEmpty(currentLeftHash))
            currentLeftHash = Salem.String.Format("left:passage/{0}/{1}/{2}/", currentPassages[0].TranslationCode, currentPassages[0].UrlBookName, currentPassages[0].ChapterNumber);

        if (currentLeftHash.indexOf("left:") == -1)
            currentLeftHash = "left:" + currentLeftHash;

        if (currentRightHash.indexOf(".html/") != -1) // .html/4 (on page 4, so swap 4 with incoming page var)
            nextRightHash = Salem.String.Format("{0}/{1}", currentRightHash.substring(0, currentRightHash.lastIndexOf("/")), page);
        else // .html (on page 1, so append incoming page arg)
            nextRightHash = Salem.String.Format("{0}/{1}", currentRightHash, page);

        nextHash = Salem.String.Format("/{0}&{1}", currentLeftHash, nextRightHash);

        // replace .html/1 with .html; .html implies first page and .html/1 is redundant
        nextHash = nextHash.replace(".html/1", ".html");

        $.history.load(nextHash.replace(/^.*#/, ''), "Right");

    },
    LoadLeft: function (hash) {
        var currentHash = location.hash;
        var currentRightHash = currentHash.split("&")[1];
        var nextHash; // we will construct this
        // incoming hash = #/passage/niv/john/3/ OR #/search/love/ (NOT #/reference/* b/c left pane is only for bible content)
        // grab the current left hash and append it before sending to $.history.load
        if (!Salem.String.IsNullOrEmpty(currentRightHash)) {
            nextHash = Salem.String.Format("/left:{0}&{1}", hash.replace("#/", ""), currentRightHash);
        } else {
            nextHash = Salem.String.Format("/left:{0}", hash.replace("#/", ""));
        }

        //Check to see if the incoming hash includes the USE-CURRENT-TRANSLATION key value.  If so we handle these links differently.
        if (nextHash.indexOf("{USE-CURRENT-TRANSLATION}") > -1) {
            //just a precautionary check to ensure that the URL is being used to view a passage
            if (nextHash.split("&")[0].indexOf("passage") > -1) {
                myBibleSvc.ValidatePassageTranslation("Left", nextHash);
            }
        }
        else {
            $.history.load(nextHash.replace(/^.*#/, ''), "Left");
            Salem.MyBible.CommunityNotes.UI.PopulateScriptureSpecificLinkText(nextHash.replace(/^.*#/, ''), "Left");
        }

        //If viewing notes for the chapter, then reload the targeted notes.
        var targettedNotesActive = $("#TargettedNotesLink").attr('class');
        if (targettedNotesActive.indexOf("active") > 0) {
            Salem.MyBible.CommunityNotes.UI.LoadCommunityNotesByScripture();
        }
    },
    GetReferenceHandler: function (url) {
        var alreadyMatched = false;
        var handler = ""; // commentary | summaries | referencearticle

        if (url.indexOf(".html") != -1) {
            handler = "ReferenceArticle";
            alreadyMatched = true;
        }

        // summaries
        var pattern = new RegExp("^/(commentaries|concordances|dictionaries|encyclopedias|history|lexicons|resources)/$");
        // ex: /commentaries/ or /concordances/ (nothing after 2nd slash)
        if (!alreadyMatched && pattern.test(url)) {
            handler = "Summaries";
            alreadyMatched = true;
        }

        // commentary
        var pattern2 = new RegExp("^/concordances/treasury-of-scripture-knowledge/$");
        // ex: /concordances/treasury-of-scripture-knowledge/
        if (!alreadyMatched && pattern2.test(url)) {
            handler = "Commentary";
            alreadyMatched = true;
        }
        var pattern3 = new RegExp("^/concordances/treasury-of-scripture-knowledge/.+/");
        // ex: /concordances/treasury-of-scripture-knowledge/genesis/
        if (!alreadyMatched && pattern3.test(url)) {
            handler = "Commentary";
            alreadyMatched = true;
        }

        // article -- BOUNCE TO EXTERNAL
        var pattern4 = new RegExp("^/concordances/strongs-exhaustive-concordance/$");
        if (!alreadyMatched && pattern4.test(url)) {
            alreadyMatched = true;
        }

        // dictionary.aspx$2 -- BOUNCE TO EXTERNAL - REG EX BROKEN!!11
        var pattern5 = new RegExp("^/(concordances|dictionaries|encyclopedias)/.+/(\\?.*)?");
        if (!alreadyMatched && pattern5.test(url)) {
            alreadyMatched = true;
        }

        // lexicon.aspx$1 -- BOUNCE TO EXTERNAL - REG EX BROKEN!!11
        var pattern6 = new RegExp("^/lexicons/.+/.+/(\\?.*)?");
        if (!alreadyMatched && pattern6.test(url)) {
            alreadyMatched = true;
        }

        if (url.indexOf("?") != -1) { // if it contains a query string param, bounce
            alreadyMatched = true;
        }

        // commentary
        var pattern7 = new RegExp("^/commentaries/(.+/)+");
        // ex: /commentaries/peoples-new-testament/matthew/
        if (!alreadyMatched && pattern7.test(url)) {
            handler = "Commentary";
            alreadyMatched = true;
        }

        // summaries
        var pattern8 = new RegExp("^/(commentaries|concordances|dictionaries|encyclopedias|history|lexicons|resources)/(.+/)+");
        if (!alreadyMatched && pattern8.test(url)) {
            handler = "Summaries";
            alreadyMatched = true;
        }

        return handler;
    },
    LoadRight: function (hash) {
        var currentHash = location.hash;
        var currentLeftHash = currentHash.split("&")[0].replace("#/", "");
        var nextHash; // we will construct this

        if (Salem.String.IsNullOrEmpty(currentLeftHash))
            currentLeftHash = Salem.String.Format("left:passage/{0}/{1}/{2}/", currentPassages[0].TranslationCode, currentPassages[0].UrlBookName, currentPassages[0].ChapterNumber);

        if (currentLeftHash.indexOf("left:") == -1)
            currentLeftHash = "left:" + currentLeftHash;

        // incoming hash = #/passage/niv/revelation/7/ OR #/reference/dictionary/agape.html OR #/search/love/
        // grab the current right hash and append it before sending to $.history.load
        nextHash = Salem.String.Format("/{0}&right:{1}", currentLeftHash, hash.replace("#/", ""));

        if (nextHash.indexOf("right:reference") != -1) {
            // some of the references should take the user to a new tab,
            // ... in which case it should NOT make it into the hash, because
            // ... we're not staying on the my-bible page
            var url = hash.replace("#/", "").substring(hash.replace("#/", "").indexOf("/"));
            var handler = Salem.HistoryManager.GetReferenceHandler(url);
            if (handler != "") // this can be handled within My-Bible
                $.history.load(nextHash.replace("{USE-CURRENT-TRANSLATION}", currentPassages[1].TranslationCode).replace(/^.*#/, ''), "Right", handler);
            else // direct user to new tab (for pages that have increased functionality, like dictionaries/lexicons
                window.open(siteRoot + url.substring(1));
        } else {
            $.history.load(nextHash.replace("{USE-CURRENT-TRANSLATION}", currentPassages[1].TranslationCode).replace(/^.*#/, ''), "Right");
        }
    },
    ProcessArg: function (arg, leftOrRight, referenceHandler) {
        var currentPassageIdx = 0; // default to left
        if (leftOrRight == "Right")
            currentPassageIdx = 1; // right

        if (arg.indexOf(leftOrRight.toLowerCase() + ":") != -1)
            arg = arg.substring(arg.indexOf(":") + 1); // get rid of "left:" or "right:"
        // arg now = passage/niv/john/3/ OR reference/dictionary/agape.html OR cms/bible-study/932352.html

        // incoming url could be http://bst.com/my-bible/#/passage/genesis/1/, sans "left:" or "right:"
        arg = arg.replace("/passage", "passage");
        // arg now = passage/niv/john/3/ OR reference/dictionary/agape.html OR cms/bible-study/932352.html

        var slashes = arg.split("/");
        var command = slashes[0];
        switch (command) {
            case 'passage':
                // set passage object
                currentPassages[currentPassageIdx].TranslationCode = slashes[1];
                currentPassages[currentPassageIdx].BookName = slashes[2];

                var digits = slashes[3]; // ex: 3:7-9 is chapter 3, verses 7 thru 9

                // ex: digits = "" for passage/niv/john/
                if (digits == "") {
                    currentPassages[currentPassageIdx].ChapterNumber = 1;
                    currentPassages[currentPassageIdx].VerseNumber = 1;
                    currentPassages[currentPassageIdx].VerseCount = 0; // entire chapter
                }
                // ex: digits = "3" for passage/niv/john/3/
                else if (digits.indexOf(":") == -1) {
                    currentPassages[currentPassageIdx].ChapterNumber = digits;
                    currentPassages[currentPassageIdx].VerseNumber = 1;
                    currentPassages[currentPassageIdx].VerseCount = 0; // entire chapter
                }
                // ex: digits = "3:4" for passage/niv/john/3:4/
                else if (digits.indexOf(":") > -1 && digits.indexOf("-") == -1) {
                    currentPassages[currentPassageIdx].ChapterNumber = digits.split(":")[0];
                    currentPassages[currentPassageIdx].VerseNumber = digits.split(":")[1];
                    currentPassages[currentPassageIdx].VerseCount = 1;
                }
                // ex: digits = "7:3-6" for passage/niv/john/7:3-6/
                else if (digits.indexOf(":") > -1 && digits.indexOf("-") > -1) {
                    currentPassages[currentPassageIdx].ChapterNumber = digits.split(":")[0];
                    currentPassages[currentPassageIdx].VerseNumber = digits.split(":")[1].split("-")[0];
                    currentPassages[currentPassageIdx].VerseCount = (digits.split(":")[1].split("-")[1] - currentPassages[currentPassageIdx].VerseNumber) + 1;
                }

                // load translation dropdown & passage
                myBibleSvc.LoadTranslationDropdown(leftOrRight);
                myBibleSvc.LoadPassage(leftOrRight);
                Salem.MyBible.SwitchView(leftOrRight + "Passage");
                break;
            case 'reference':
                var handler = referenceHandler;
                // if ref handler hasn't been set yet via LoadLeft
                if (typeof handler == 'undefined') {
                    handler = Salem.HistoryManager.GetReferenceHandler(arg.substring(arg.indexOf("/")));
                }
                myBibleSvc.LoadReference(arg.substring(arg.indexOf("/")), handler); // ('/dictionary/agape.html' or '/commentaries/bob/')
                Salem.MyBible.SwitchView(leftOrRight + "Article");
                break;
            case 'strongs':
                myBibleSvc.LoadStrongsArticle(arg.substring(arg.indexOf("/") + 1)); // myBibleSvc.LoadReferenceArticle('/157/english/.../')
                Salem.MyBible.SwitchView(leftOrRight + "Article");
                // similar as above
                break;
            case 'cms':
                myBibleSvc.LoadCmsArticle(arg.substring(arg.indexOf("/"))); // myBibleSvc.LoadCmsArticle('/bible-study/23523.html')
                Salem.MyBible.SwitchView(leftOrRight + "Article");
                break;
        } // end switch        
    },

    Callback: function (hash, leftOrRight, referenceHandler) {
        // hash value examples:
        // "" (no hash)
        // /left:passage/niv/john/3/&right:passage/nlt/galatians/5/
        // /left:passage/niv/john/3/&right:reference/dictionary/agape.html

        if (hash) {
            $("a").blur(); // blur out whatever link button was clicked

            var args = hash.split("&"); // always assume 2 args

            switch (leftOrRight) {
                case "Left":
                    Salem.HistoryManager.ProcessArg(args[0], leftOrRight, referenceHandler);
                    break;
                case "Right":
                    Salem.HistoryManager.ProcessArg(args[1], leftOrRight, referenceHandler);
                    break;
                default:
                    Salem.HistoryManager.ProcessArg(args[0], "Left", referenceHandler);
                    if (args.length == 2)
                        Salem.HistoryManager.ProcessArg(args[1], "Right", referenceHandler);
                    else if (args.length == 1) {
                        // only left arg is provided, so load right with default passage
                        // ...so when user clicks on parallel view there is something there
                        currentPassages[1].TranslationCode = defaultTranslation; // defaultTranslation defined in /my-bible/default.aspx
                        currentPassages[1].BookName = "genesis";
                        currentPassages[1].ChapterNumber = 1;
                        currentPassages[1].VerseNumber = 1;
                        currentPassages[1].VerseCount = 0;
                        myBibleSvc.LoadTranslationDropdown("Right");
                        myBibleSvc.LoadPassage("Right");
                        Salem.MyBible.SwitchView("Default");
                    }
            }
        } else { // no hash, must be simply /my-bible/
            // set both current passages to genesis/1
            for (i = 0; i < 2; i++) {
                currentPassages[i].TranslationCode = defaultTranslation; // defaultTranslation defined in /my-bible/default.aspx
                currentPassages[i].BookName = "genesis";
                currentPassages[i].ChapterNumber = 1;
                currentPassages[i].VerseNumber = 1;
                currentPassages[i].VerseCount = 0;
            }
            myBibleSvc.LoadTranslationDropdown("Left");
            myBibleSvc.LoadTranslationDropdown("Right");
            myBibleSvc.LoadPassage("Left");
            myBibleSvc.LoadPassage("Right");
            Salem.MyBible.SwitchView("Default");
        } // end: hash or no hash
    } // end: Callback: function(hash, leftOrRight)
};

// can these be right-left agnostic?
function BindRightColumnButtons() {
    $("#ParallelViewTab .ButtonBar .Highlight").click(function() { RightButtonBarClicked(); });
    $("#ParallelViewTab .ButtonBar .Bookmark").click(function() { RightButtonBarClicked(); });
    $("#ParallelViewTab .ButtonBar #PrintButton").unbind('click').click(function() { RightColumnPrintSetup(); return false; });
    $("#ParallelViewTab .ButtonBar #AudioButton").hide(); //hide the audio button for now
}

// can this be right-left agnostic?
function RightButtonBarClicked() {
    column = "Right";
}

// can this be right-left agnostic?
function RightColumnPrintSetup() {
    //Need to fix the styling of output
    var title = $("#RightColumn .Passage-Title").html();
    var body = $("#RightColumn #PassageContainer").html(); //$("#ParallelViewTab .ButtonBar #PrintButton").attr("");
    var cssClass = 'Print GlobalPrintLink';
    var fontsize = $($("#RightColumn .ButtonBar #PrintButton").attr("printbody")).css("font-size");
    printPage(title, body, cssClass, fontsize);
}

Salem.RegisterNamespace("MyBible.Service");
Salem.MyBible.Service = function() {
    var Instance = this;
    this.Service = new Salem.Ajax.Service(myBibleProxyUrl);

    this.LoadReference = function(url, referenceHandler) {
        $("#RightColumn #ArticleView").html(Salem.String.Format('<img src="{0}" />', ajaxLoaderImgSrc));
        this.Service.JsonGet("Get" + referenceHandler, { url: url }, this.FillArticle, this.FillArticle_Failure);
    };

    this.LoadStrongsArticle = function(url) {
        $("#RightColumn #ArticleView").html(Salem.String.Format('<img src="{0}" />', ajaxLoaderImgSrc));
        this.Service.JsonGet("GetStrongsArticle", { url: url }, this.FillArticle, this.FillArticle_Failure);
    };

    this.LoadCmsArticle = function(url) {
        // url = /bible-study/2354.html OR /bible-study/2354.html/2 (page 2)
        $("#RightColumn #ArticleView").html(Salem.String.Format('<img src="{0}" />', ajaxLoaderImgSrc));
        var articleId = 0;
        var activePage = 1;
        if (url.indexOf(".html/") != -1) {
            activePage = url.substring(url.lastIndexOf("/") + 1);
            // now that we have the active page to display, remove it from the url so we can parse out the articleid knowing it's after the last slash
            url = url.substring(0, url.lastIndexOf("/"));
        }
        articleId = url.substring(url.lastIndexOf("/") + 1).replace(".html", "");
        this.Service.JsonGet("GetCmsArticle", { articleId: articleId, activePage: activePage }, this.FillArticle, this.FillArticle_Failure);
    };

    this.FillArticle = function(ret) {
        $("#RightColumn #ArticleView").html(ret);
        $('html, body').animate({ scrollTop: 0 }, 1000);

        var idPrefix = "#RightColumn #ArticleView ";
        $(idPrefix + ".GlobalPrintLink").click(function() {
            var title = $(idPrefix + $(this).attr("printtitle")).html();
            var body = $(idPrefix + $(this).attr("printbody")).html(); //$("#ParallelViewTab .ButtonBar #PrintButton").attr("");
            var cssClass = 'Print GlobalPrintLink'; // ?
            var fontsize = $(idPrefix + $(this).attr("printbody")).css("font-size");
            printPage(title, body, cssClass, fontsize);
        });
    };

    this.LoadPassage = function(leftOrRight) {
        this.StripAllButPrimaryKey(leftOrRight);
        // clear any previous selections
        Salem.MyBible.Passage.UI.DeselectAll(leftOrRight);

        if (leftOrRight == 'Left') {
            $("#LeftColumn .Passage-Text").html("");
            $("#LeftColumn .Passage-Title").html(Salem.String.Format('<img src="{0}" />', ajaxLoaderImgSrc));
            this.Service.JsonGet("GetPassage", {
                myPsg: JSON.stringify(currentPassages[0])
                , leftOrRight: leftOrRight
            }, this.FillLeftPassage, function() { Instance.LoadPassage_Failure(leftOrRight); });
        }
        if (leftOrRight == 'Right') {
            $("#RightColumn .Passage-Text").html("");
            $("#RightColumn .Passage-Title").html(Salem.String.Format('<img src="{0}" />', ajaxLoaderImgSrc));
            this.Service.JsonGet("GetPassage", {
                myPsg: JSON.stringify(currentPassages[1])
                , leftOrRight: leftOrRight
            }, this.FillRightPassage, function() { Instance.LoadPassage_Failure(leftOrRight); });
        }
    };

    this.LoadTranslationDropdown = function(leftOrRight, desiredTranslation) {
        var currentPassageIdx = 0;
        if (leftOrRight == "Right")
            currentPassageIdx = 1;
        this.Service.JsonGet("GetTranslationDropdown", {
            newTranslation: currentPassages[currentPassageIdx].TranslationCode
        }, function(ret) { Instance.FillTranslationDropdown(ret, leftOrRight); }, null);
    };

    this.FillLeftPassage = function(ret) {
        Instance.FillPassage(ret, "Left");
        GetAudioInformation(ret.HasAudio, ret.AudioUrl, ret.AudioStream, "Left", ret.AudioIcon, ret.AudioLink);
    };

    this.FillRightPassage = function(ret) {
        Instance.FillPassage(ret, "Right");
    };

    this.LoadPassage_Failure = function(leftOrRight) {
        var idPrefix = Salem.String.Format("#{0}Column ", leftOrRight);
        $(idPrefix + ".Passage-Text").html("Error loading content for MyBible, please try again.");
    };

    this.FillArticle_Failure = function() {
        alert('Unable to load article. Please try again.');
    };

    this.LoadDisplayOptions = function(leftOrRight) {
        this.StripAllButPrimaryKey(leftOrRight);
        var currentPassageIdx = 0;
        if (leftOrRight == "Right")
            currentPassageIdx = 1;

        this.Service.JsonGet("GetDisplayData", {
            myPassageObj: JSON.stringify(currentPassages[currentPassageIdx])
        }, function(ret) { Instance.FillDisplayOptions(ret, leftOrRight); }, this.LoadDisplayOptions_Failure);

    };

    this.FillDisplayOptions = function(ret, leftOrRight) {
        var idPrefix = Salem.String.Format("#{0}Column ", leftOrRight);
        $(idPrefix + "div.DisplayButton").CheckboxList(ret.checkboxes);
        Salem.MyBible.Passage.UI.BindDisplayOptions(leftOrRight);
    };

    this.LoadDisplayOptions_Failure = function() {
        // silent
    };

    // prevents get requests from having too big of query strings
    this.StripAllButPrimaryKey = function(leftOrRight) {
        var currentPassageIdx = 0;
        if (leftOrRight == "Right")
            currentPassageIdx = 1;
        currentPassages[currentPassageIdx].PassageTitle = "";
        currentPassages[currentPassageIdx].PassageHtml = "";
    };

    this.FillTranslationDropdown = function(dropdownHtml, leftOrRight) {
        var idPrefix = Salem.String.Format("#{0}Column ", leftOrRight);

        $(idPrefix + ".Translation-Dropdown-Placeholder").html(dropdownHtml);
        $(idPrefix + ".Translation").change(function() {
            // load new translation (push to Salem.HistoryManager.Load function)
            if ($(this).val() != "Separator")
                Salem.HistoryManager.LoadNewTranslation($(this).val(), leftOrRight)
        });
        // set change event
    };

    this.FillPassage = function(myPassageObj, leftOrRight) {
        Instance.UpdateCurrentPsg(myPassageObj, leftOrRight);
        var idPrefix = Salem.String.Format("#{0}Column ", leftOrRight);

        $(idPrefix + ".Passage-Title").html(myPassageObj.PassageTitle);
        $(idPrefix + ".Passage-Text").html(myPassageObj.PassageHtml);

        // update alt / title attr's for next/prev links
        UpdateElementAttributes(idPrefix + ".PreviousBook", "Previous Book - " + myPassageObj.PrevBookTitle);
        UpdateElementAttributes(idPrefix + ".NextBook", "Next Book - " + myPassageObj.NextBookTitle);
        UpdateElementAttributes(idPrefix + ".PreviousChapter", "Previous Chapter - " + myPassageObj.PrevChapterTitle);
        UpdateElementAttributes(idPrefix + ".NextChapter", "Next Chapter - " + myPassageObj.NextChapterTitle);

        // set href's for next/prev links
        var href1 = Salem.String.Format("#/passage/{0}/{1}/", myPassageObj.TranslationCode, myPassageObj.PrevUrlBookName);
        if (leftOrRight == "Left") {
            $(idPrefix + ".PreviousBook").unbind('click').click(function() {
                Salem.HistoryManager.LoadLeft(href1);
                return false;
            });
        } else {
            $(idPrefix + ".PreviousBook").unbind('click').click(function() {
                Salem.HistoryManager.LoadRight(href1);
                return false;
            });
        }

        var href2 = Salem.String.Format("#/passage/{0}/{1}/", myPassageObj.TranslationCode, myPassageObj.NextUrlBookName);
        if (leftOrRight == "Left") {
            $(idPrefix + ".NextBook").unbind('click').click(function() {
                Salem.HistoryManager.LoadLeft(href2);
                return false;
            });
        } else {
            $(idPrefix + ".NextBook").unbind('click').click(function() {
                Salem.HistoryManager.LoadRight(href2);
                return false;
            });
        }

        var href3 = Salem.String.Format("#/passage/{0}/{1}/", myPassageObj.TranslationCode, myPassageObj.PrevUrlChapterTitle);
        if (leftOrRight == "Left") {
            $(idPrefix + ".PreviousChapter").unbind('click').click(function() {
                Salem.HistoryManager.LoadLeft(href3);
                return false;
            });
        } else {
            $(idPrefix + ".PreviousChapter").unbind('click').click(function() {
                Salem.HistoryManager.LoadRight(href3);
                return false;
            });
        }

        var href4 = Salem.String.Format("#/passage/{0}/{1}/", myPassageObj.TranslationCode, myPassageObj.NextUrlChapterTitle);
        if (leftOrRight == "Left") {
            $(idPrefix + ".NextChapter").unbind('click').click(function() {
                Salem.HistoryManager.LoadLeft(href4);
                return false;
            });
        } else {
            $(idPrefix + ".NextChapter").unbind('click').click(function() {
                Salem.HistoryManager.LoadRight(href4);
                return false;
            });
        }

        var href5 = Salem.String.Format("#/passage/{0}/{1}/{2}/", myPassageObj.TranslationCode, myPassageObj.UrlBookName, myPassageObj.ChapterNumber);
        if (leftOrRight == "Left") {
            $(idPrefix + ".FullChapter").unbind('click').click(function() {
                Salem.HistoryManager.LoadLeft(href5);
                return false;
            });
        } else {
            $(idPrefix + ".FullChapter").unbind('click').click(function() {
                Salem.HistoryManager.LoadRight(href5);
                return false;
            });
        }

        GetAudioInformation(myPassageObj.HasAudio, myPassageObj.AudioUrl, myPassageObj.AudioStream, "Left", myPassageObj.AudioIcon, myPassageObj.AudioLink);
        myBibleSvc.LoadDisplayOptions(leftOrRight);
        Salem.MyBible.Passage.UI.BindResourceTabs(leftOrRight);


    };

    this.ValidatePassageTranslation = function(leftOrRight, nextHash) {
        //retrieve the default translation for a passage: this will be used in the event that the
        //user selects a link in the right hand column that does NOT exist in the current translation.
        //If this is the case the passage will be loaded in the translation that the user used to 
        // bookmark, highlight, etc. the scripture
        var defaultPassageTranslation = nextHash.split("&")[0].split("DefaultTranslation-")[1];

        //parse nextHash
        var translationCode = currentPassages[0].TranslationCode;
        var bookName = nextHash.split("&")[0].replace('#/', '').split("/")[3];
        var chapterNumber = nextHash.split("&")[0].replace('#/', '').split("/")[4].split(":")[0];
        var verseNumber = nextHash.split("&")[0].replace('#/', '').split("/")[4].split(":")[1];
        var newTranslationCode;
        var verseStart; //will be used to calculate verseCount and will be passed into the service call as verseNumber
        var verseEnd; //will be used to calculate verseCount
        var verseCount;

        //determine whether there is a single verse or multiple verses
        if (verseNumber.indexOf('-') > -1) {
            //there are multiple verses so we parse these out and calculate verse count
            verseStart = verseNumber.split('-')[0];
            verseEnd = verseNumber.split('-')[1];
            verseCount = parseInt(verseEnd - verseStart);
        }
        else {
            //set verseStart = verseNumber this is the value we will pass as a parameter in both cases
            verseStart = verseNumber;
            verseCount = 1;
        }

        var data = { translationCode: translationCode, defaultTranslation: defaultPassageTranslation, bookName: bookName, chapterNumber: chapterNumber, verseStart: verseStart, verseCount: verseCount };
        this.Service.JsonPost("ValidatePassageTranslation", JSON.stringify(data), function(ret) { Instance.ValidatePassageTranslation_Success(ret, leftOrRight, defaultPassageTranslation, nextHash); }, null);
    };

    this.ValidatePassageTranslation_Success = function(ret, leftOrRight, defaultPassageTranslation, nextHash) {
        var translationCode = ret.toString();
        var idPrefix = "#" + leftOrRight + "Column ";  // default to left side
        //check to see if a defaultTranslation value has been returned
        //if so the current passage does not exist in the current translation
        //so we use the retrieved translation to populate the passage
        if (!Salem.String.IsNullOrEmpty(translationCode)) {//the current passage does not exist in this translation
            $.history.load(nextHash.replace("{USE-CURRENT-TRANSLATION}", translationCode).replace("DefaultTranslation-" + defaultPassageTranslation, '').replace(/^.*#/, ''), "Left");
            $(idPrefix).find(".Translation").val(translationCode)
            Salem.MyBible.CommunityNotes.UI.PopulateScriptureSpecificLinkText(nextHash.replace(/^.*#/, ''), "Left");
        }
        else {
            //the current passage exists in the current translation so we load the passage using the current translation
            $.history.load(nextHash.replace("{USE-CURRENT-TRANSLATION}", currentPassages[0].TranslationCode).replace("DefaultTranslation-" + defaultPassageTranslation, '').replace(/^.*#/, ''), "Left");
            Salem.MyBible.CommunityNotes.UI.PopulateScriptureSpecificLinkText(nextHash.replace(/^.*#/, ''), "Left");
        }
    };

    this.UpdateCurrentPsg = function(myPassageObj, leftOrRight) {
        var currentPassageIdx = 0; // default to left
        if (leftOrRight == "Right")
            currentPassageIdx = 1;

        currentPassages[currentPassageIdx].PassageTitle = myPassageObj.PassageTitle;
        currentPassages[currentPassageIdx].TranslationCode = myPassageObj.TranslationCode;
        currentPassages[currentPassageIdx].PrevBookTitle = myPassageObj.PrevBookTitle;
        currentPassages[currentPassageIdx].PrevUrlBookName = myPassageObj.PrevUrlBookName;
        currentPassages[currentPassageIdx].BookName = myPassageObj.BookName;
        currentPassages[currentPassageIdx].BookAbbreviation = myPassageObj.BookAbbreviation;
        currentPassages[currentPassageIdx].BookCode = myPassageObj.BookCode;
        currentPassages[currentPassageIdx].UrlBookName = myPassageObj.UrlBookName;
        currentPassages[currentPassageIdx].NextBookTitle = myPassageObj.NextBookTitle;
        currentPassages[currentPassageIdx].NextUrlBookName = myPassageObj.NextUrlBookName;
        currentPassages[currentPassageIdx].PrevChapterTitle = myPassageObj.PrevChapterTitle;
        currentPassages[currentPassageIdx].PrevUrlChapterTitle = myPassageObj.PrevUrlChapterTitle;
        currentPassages[currentPassageIdx].ChapterNumber = myPassageObj.ChapterNumber;
        currentPassages[currentPassageIdx].NextChapterTitle = myPassageObj.NextChapterTitle;
        currentPassages[currentPassageIdx].NextUrlChapterTitle = myPassageObj.NextUrlChapterTitle;
        currentPassages[currentPassageIdx].VerseNumber = myPassageObj.VerseNumber;
        currentPassages[currentPassageIdx].VerseCount = myPassageObj.VerseCount;
        currentPassages[currentPassageIdx].PassageHtml = myPassageObj.PassageHtml;
        currentPassages[currentPassageIdx].HasAudio = myPassageObj.HasAudio;
        currentPassages[currentPassageIdx].AudioUrl = myPassageObj.AudioUrl;
        currentPassages[currentPassageIdx].AudioStream = myPassageObj.AudioStream;
        currentPassages[currentPassageIdx].DefaultPassageTranslation = myPassageObj.DefaultPassageTranslation;
    };
}                                                                                // Salem.MyBible.Service

var UpdateElementAttributes = function(selector, val) {
    $(selector).attr("title", val);
};


function GetAudioInformation(hasAudio, audioUrl, audioStream, leftOrRight, iconUrl, linkUrl) {
    var idPrefix = "#LeftColumn ";  // default to left side

    if (leftOrRight == "Left" || leftOrRight == "both") {
        $(idPrefix + "#MyBiblePlayer").hide().html('');
        if (hasAudio == true || hasAudio == 'True') {
            $(idPrefix + "#AudioButton").removeClass('NoAudio').addClass('Audio');
            $(idPrefix + "#AudioButton").toggle(function() {
                PlayChapterAudio(audioUrl.toString(), 'MyBiblePlayer', playerUrl, playerSkin, audioStream.toString(), iconUrl, linkUrl);
            }, function() {
                $(idPrefix + "#MyBiblePlayer").css('padding-bottom', 0).hide().html('');
            });
        }
        else {
            $(idPrefix + "#AudioButton").removeClass('Audio').addClass('NoAudio');
            $(idPrefix + "#AudioButton").unbind('click');
        }
    }
}



var currentPassages = [];
var myBibleSvc;

$(document).ready(function() {
    myBibleSvc = new Salem.MyBible.Service();
    currentPassages[0] = JSON.parse(MyPassageModel); // empty passage for left side
    currentPassages[1] = JSON.parse(MyPassageModel); // empty passage for right side

    // ?? can this be left-or-right-agnostic?
    BindRightColumnButtons();

    Salem.MyBible.Navigation.UI.Init(); // Initialize controls in the right pane, defined in MyBible-Navigation.js

    Salem.MyBible.Notes.UI.Init();

    Salem.MyBible.ButtonBar.Init();

    Salem.MyBible.Search.UI.Init();

    Salem.HistoryManager.Init(); // Salem.HistoryManager.Callback is called, column var is set to left|right|both    
});