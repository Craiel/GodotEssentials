'use strict';

const SectionBrackets = ['[', ']'];

const ParameterType = Object.freeze({
    Quoted: 1,
    Unquoted: 2
});

const ParameterParseMode = Object.freeze({
    Key: 1,
    Value: 2
});

class GodotFileSection {
    constructor(id) {

        this.id = id;
        this.parameters = {};
        this.parameterType = {};
        this.parameterOrder = [];
        this.content = [];
    }

    loadFromData(data) {
        this.parse(data);

        if(this.id === undefined || this.id === "") {
            throw "Invalid Section ID: " + data;
        }
    }

    parse(data) {
        let idIndex = data[0] === SectionBrackets[0] ? 1 : 0;
        let idLength = data.indexOf(' ');
        if(idLength < 0) {
            this.id = data;
            for(let i = 0; i < SectionBrackets.length; i++) {
                this.id = this.id.replace(SectionBrackets[i], '');
            }

            return;
        }

        this.id = data.substring(idIndex, idLength);

        let paramEnd = data.endsWith(SectionBrackets[1]) ? data.length - 1 : data.length;
        let paramContent = data.substring(idLength + 1, paramEnd);

        let currKey = '';
        let currVal = '';
        let inQuotes = false;
        let inBrackets = false;
        let m = ParameterParseMode.Key;
        for(let ip = 0; ip < paramContent.length; ip++) {
            let c = paramContent[ip];
            switch(m){
                case ParameterParseMode.Key: {
                    if(c === '=') {
                        m = ParameterParseMode.Value;
                    } else {
                        currKey += c;
                    }

                    break
                }

                case ParameterParseMode.Value: {
                    currVal += c;
                    if(c === '"') {
                        if(inQuotes === true) {
                            inQuotes = false;
                        } else {
                            inQuotes = true;
                        }

                        continue;
                    }

                    if(c === '(') { inBrackets = true; }

                    if(c === ')') { inBrackets = false; }

                    if(c === ' ' && inQuotes === false && inBrackets === false) {
                        m = ParameterParseMode.Key;
                        this.addParam(currKey, currVal);
                        currKey = '';
                        currVal = '';
                    }

                    break
                }
            }
        }

        if(currKey !== '') {
            this.addParam(currKey, currVal);
        }
    }

    addQuotedParam(id, value) {
        this.addParam(id, value, ParameterType.Quoted);
    }

    addParam(id, value, type) {
        value = value.trim();
        if(type === undefined) {
            if(value.charAt(0) === '"' && value.charAt(value.length -1) === '"'){
                type = ParameterType.Quoted;
                value = value.substr(1,value.length -2);
            } else {
                type = ParameterType.Unquoted;
            }
        }

        this.parameterType[id] = type ?? ParameterType.Quoted;
        this.parameters[id] = value;
        this.parameterOrder.push(id);
    }

    addContent(key, value) {
        this.content.push([key, value]);
    }

    hasParam(key) {
        return this.parameters[key] !== undefined;
    }

    getParamValue(key) {
        return this.parameters[key];
    }

    save(target) {
        let idLine = SectionBrackets[0] + this.id;
        for(let i = 0; i < this.parameterOrder.length; i++) {
            let paramKey = this.parameterOrder[i];
            switch (this.parameterType[paramKey]) {
                case ParameterType.Quoted:{
                    idLine += ' ' + paramKey + '=\"' + this.parameters[paramKey] + '\"';
                    break;
                }

                case ParameterType.Unquoted: {
                    idLine += ' ' + paramKey + '=' + this.parameters[paramKey] + '';
                    break;
                }
            }
        }

        idLine += SectionBrackets[1];
        target.push(idLine);

        for(let i = 0; i < this.content.length; i++) {
            let contentKey = this.content[i][0];
            let contentValue = this.content[i][1];
            target.push(contentKey + ' = ' + contentValue);
        }

        target.push('');
    }
}

module.exports = {
    create: function(id) {
        return new GodotFileSection(id);
    },
    createFromData: function(data) {
        let result = new GodotFileSection();
        result.loadFromData(data);
        return result;
    }
};