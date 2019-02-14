/**
 * @license
 * Copyright © 2015 Intuilab
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * The Software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability,
 * fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability,
 * whether in an action of contract, tort or otherwise, arising from, out of or in connection with the Software or the use or other dealings in the Software.
 *
 * Except as contained in this notice, the name of Intuilab shall not be used in advertising or otherwise to promote the sale,
 * use or other dealings in this Software without prior written authorization from Intuilab.
 */


TextManipulationBC.prototype = new Object();        // Here's where the inheritance occurs
TextManipulationBC.prototype.constructor = TextManipulationBC;


function TextManipulationBC() {
}




/**
 * Concatenate a text to the IA text
 * @param {String} text
 */
TextManipulationBC.prototype.concatenate = function (input, text, position) {
    if (input == null) return "";
    else if (input === ""){
        return text;
    }
    else{
        if (position == "before")
            return text + input;
        else
            return input + text;
    }
};


/**
 * enclose a IA text between a before and after text
 * @param {String} text
 */
TextManipulationBC.prototype.enclose = function (input, before, after) {
    if (input == null) return "";
    else if (input === ""){
        return before + after;
    }
    else{
		return before + input + after;        
    }
};

/**
 * Replace a text from the IA text with another one
 * @param {String} oldText
 * @param {String} newText
 */
TextManipulationBC.prototype.replace = function (input, oldText, newText) {
    if ((input == null) || (input === "")) return "";
    else {
        var regex = new RegExp(oldText, "g");
        var replace = input.replace(regex, newText);
        return replace;
    }
};

/**
 * Extract a substring between two indexes from the IA text
 * @param {Number} startIndex 1 to x
 * @param {Number} endIndex   1 to x
 */
TextManipulationBC.prototype.substring = function (input, startIndex, endIndex) {
    if ((input == null) || (input === "")) return "";
    else {
        var substring = input.substring(startIndex - 1, endIndex);
        return substring;
    }
};

/**
 * Split the IA text according to a separator and return the 'index'th item of the split result
 * @param {String} separator
 * @param {Number} index 1 to x
 */
TextManipulationBC.prototype.itemAt = function (input, separator, index) {
    if ((input == null) || (input === "")) return "";
    else {
        var selectedSeparator = " ";

        switch (separator) {
            case "blank":
                selectedSeparator = " ";
                break;
            case "comma":
                selectedSeparator = ",";
                break;
            case "semicolon":
                selectedSeparator = ";";
                break;
            case "dash":
                selectedSeparator = "-";
                break;
            case "pipe":
                selectedSeparator = "|";
                break;
            case "underscore":
                selectedSeparator = "_";
                break;
        }

        var split = input.split(selectedSeparator)[index - 1];
        return split;
    }
};


/**
 * Convert the string to lowercase letters
 */
TextManipulationBC.prototype.toLowerCase = function (input) {
    if ((input == null) || (input === "")) return "";
    else {
        var lowerCase = input.toLowerCase();

        return lowerCase;
    }
};

/**
 * Convert the string to an uppercase pattern
 * @param {String} pattern
 */
TextManipulationBC.prototype.toUpperCase = function (input, pattern) {
    if ((input == null) || (input === "")) return "";
    else {
        switch (pattern) {
            case "all":
                return this._fullUpperCase(input);
                break;
            case "firstLetter":
                return this._firstLetterUppercase(input);
                break;
            case "properName":
                return this._properName(input);
                break;
            default :
                return input
                break;
        }
    }
};

/**
 * Convert the string to uppercase letters
 * @private
 */
TextManipulationBC.prototype._fullUpperCase = function (input) {
    if ((input == null) || (input === "")) return "";
    else {
        return input.toUpperCase();
    }
};

/**
 * Convert only the first letter to uppercase
 * @private
 */
TextManipulationBC.prototype._firstLetterUppercase = function (input) {
    if ((input == null) || (input === "")) return "";
    else {
        var firstChar = input.charAt(0);
        var firstCharToUpperCase = firstChar.toUpperCase();
        var textLessFirstChar = input.substring(1);
        var textLessFirstCharToLowerCase = textLessFirstChar.toLowerCase();
        var properName = firstCharToUpperCase + textLessFirstCharToLowerCase;
        return properName;
    }
};

/**
 * Convert first letter of every word to uppercase
 * @private
 */
TextManipulationBC.prototype._properName = function (input) {
    if ((input == null) || (input === "")) return "";
    else {
        var words = input.split(" ");
        var result = "";
        for (var i = 0; i < words.length; i++) {
            var temp = words[i].toLowerCase();
            temp = temp.charAt(0).toUpperCase() + temp.substring(1);
            result = result + temp;
            if (i < words.length - 1) {
                result = result + " ";
            }
        }
        return result;
    }
};