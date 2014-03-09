/* This script and many more are available free online at
The JavaScript Source!! http://javascript.internet.com
Created by: Sandeep Gangadharan | http://www.sivamdesign.com/scripts/ */
if (document.getElementById) {
    document.write('<style type="text/css">.texter {display:none; border-left:white 20px solid; color:#404040; font: .9em verdana, arial, helvetica, sans-serif; margin-bottom: 12px;}</style>')
}

var divNum = new Array(
"a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8", "a9", "a10", 
"a11", "a12", "a13", "a14", "a15", "a16", "a17", "a18", "a19", "a20", 
"a21", "a22", "a23", "a24", "a25", "a26", "a27", "a28", "a29", "a30",  
"a31", "a32", "a33", "a34", "a35", "a36", "a37", "a38", "a39", "a40", 
"a41", "a42", "a43", "a44", "a45", "a46", "a47", "a48", "a49", "a50",
"a51", "a52", "a53", "a54", "a55");

function openClose(theID) {
    for (var i = 0; i < divNum.length; i++) {
        if (divNum[i] == theID) {
            if (document.getElementById(divNum[i]).style.display == "block") { document.getElementById(divNum[i]).style.display = "none" }
            else {
                document.getElementById(divNum[i]).style.display = "block"
            }
        } else {
            document.getElementById(divNum[i]).style.display = "none";
        }
    }
}