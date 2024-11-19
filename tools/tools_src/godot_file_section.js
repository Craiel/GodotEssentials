'use strict';

const SectionParamRegex = new RegExp('(\\w+)[\\s]*=[\\s]*((?:[^"\'\\s]+)|\'(?:[^\']*)\'|"(?:[^"]*)")', 'g');
const SectionBrackets = ['[', ']'];

const ParameterType = Object.freeze({
    Quoted: 1,
    Unquoted: 2
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
        let paramMatch;
        while(paramMatch = SectionParamRegex.exec(paramContent))
        {
            let id = paramMatch[1].trim();
            let value = paramMatch[2].trim();

            // console.log(this.id + '.' + id + ' -> ' + value);
            if(id === undefined || value === undefined) {
                throw "Invalid Section Arguments: " + paramMatch[0];
            }

            if(value.startsWith('"')) {
                value = value.replaceAll('"', '');
                this.parameterType[id] = ParameterType.Quoted;
            } else {
                this.parameterType[id] = ParameterType.Unquoted;
            }
            this.parameters[id] = value;
            this.parameterOrder.push(id);
        }
    }

    addParam(id, value, type) {
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