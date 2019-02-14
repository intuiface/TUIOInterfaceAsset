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


EvalBC.prototype = new Object();          // Here's where the inheritance occurs
EvalBC.prototype.constructor = EvalBC;

/**
 * @constructor
 */
function EvalBC() {

}


/**
 * @method
 */
EvalBC.prototype.evalBC = function (input, evalFunction) {

    if ((evalFunction == undefined) || (evalFunction == "")){
        return null;
    }

    try{
        var INPUT;
        if (input == null){
            INPUT = null;
        }
        else{
            INPUT = this._stringToBoolean(input);
            if (INPUT == null){
                INPUT = this._stringToFloat(input);
            }
            if (INPUT == null){
                INPUT = input.toString();
            }
        }

        return eval(evalFunction);

    }
    catch (e){
        console.error("Binding Converter Eval catch SyntaxError: "+ e.message);
        return null;
    }


};



EvalBC.prototype._stringToBoolean = function(string){
    switch(string.toString().toLowerCase()){
        case "true": return true;
        case "false": return false;
        default: return null;
    }
};

EvalBC.prototype._stringToFloat = function(string){

    var result = parseFloat(string);
    if (isNaN(result)){
        return null;
    }
    else if (result.toString() == string){
        return result;
    }
    else {
        return null;
    }
};