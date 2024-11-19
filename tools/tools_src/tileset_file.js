'use strict';

const fs = require("fs");
const path = require("path");
const { GodotFile } = require('./godot_file.js');
const createSection = require('./godot_file_section.js').create;

class TilesetFile extends GodotFile {
    constructor(src) {
        super(src);
    }

    loadNavData(file) {
        let data = JSON.parse(fs.readFileSync(file));

        // We have to delete all previous polygons since godot re-assigns them on modification
        let navPolygons = this.getSections('sub_resource', 'type', 'NavigationPolygon');
        for(let i = 0; i < navPolygons.length; i++) {
            let polygon = navPolygons[i];
            this.deleteSection(polygon);
        }

        let sectionInsertIndex = this.getLastIndexOfSection('ext_resource') + 1;
        for(let polyId in data.poly) {
            let polySection = createSection();
            polySection.id = 'sub_resource';
            polySection.addParam('type', 'NavigationPolygon');
            polySection.addParam('id', polyId);
            this.sections.splice(sectionInsertIndex, 0, polySection);

            // Overwrite the polygon data
            polySection.content = JSON.parse(data.poly[polyId]);
        }

        let atlasSections = this.getSections('sub_resource', 'type', 'TileSetAtlasSource');
        if(atlasSections.length === 0) {
            throw "No Atlas to apply to found";
        }

        for(let i = 0; i < atlasSections.length; i++) {
            let atlas = atlasSections[i];
            let entriesToDelete = [];
            for(let i = 0; i < atlas.content.length; i++) {
                let tileInfo = atlas.content[i][0].split('/');
                if (tileInfo.length < 3 || tileInfo[2].indexOf('navigation_layer') < 0) {
                    continue;
                }

                entriesToDelete.push(i);
            }

            // Get rid of old physics entries
            for(let i = entriesToDelete.length - 1; i >= 0; i--) {
                atlas.content.splice(entriesToDelete[i], 1);
            }

            for (let lineKey in data.tile) {
                atlas.content.push([lineKey, data.tile[lineKey]]);
            }
        }
    }

    saveNavData(file, atlasIndex = 0) {
        let output = {
            poly: {},
            tile: {}
        };

        let navPolygons = this.getSections('sub_resource', 'type', 'NavigationPolygon');
        for(let i = 0; i < navPolygons.length; i++) {
            let polygon = navPolygons[i];
            let polyId = polygon.getParamValue("id");
            output.poly[polyId] = JSON.stringify(polygon.content);
        }

        let atlasSections = this.getSections('sub_resource', 'type', 'TileSetAtlasSource');
        if(atlasSections.length === 0 || atlasSections.length <= atlasIndex) {
            throw "Atlas to save not found: " + atlasSections.length + " != " + atlasIndex;
        }

        let atlas = atlasSections[atlasIndex];
        for(let i = 0; i < atlas.content.length; i++) {
            let tileInfo = atlas.content[i][0].split('/');
            if(tileInfo.length < 3 || tileInfo[2].indexOf('navigation_layer') < 0) {
                continue;
            }

            output.tile[atlas.content[i][0]] = atlas.content[i][1];
        }

        fs.writeFileSync(file, JSON.stringify(output, null, 2));
    }

    loadPhysicsData(file) {
        let data = JSON.parse(fs.readFileSync(file));

        let atlasSections = this.getSections('sub_resource', 'type', 'TileSetAtlasSource');
        if(atlasSections.length === 0) {
            throw "No Atlas to apply to found";
        }

        for(let i = 0; i < atlasSections.length; i++) {
            let atlas = atlasSections[i];
            let entriesToDelete = [];
            for(let i = 0; i < atlas.content.length; i++) {
                let tileInfo = atlas.content[i][0].split('/');
                if (tileInfo.length < 3 || tileInfo[2].indexOf('physics_layer') < 0) {
                    continue;
                }

                entriesToDelete.push(i);
            }

            // Get rid of old physics entries
            for(let i = entriesToDelete.length - 1; i >= 0; i--) {
                atlas.content.splice(entriesToDelete[i], 1);
            }

            for (let lineKey in data.tile) {
                atlas.content.push([lineKey, data.tile[lineKey]]);
            }
        }
    }

    savePhysicsData(file, atlasIndex = 0) {
        let output = {
            tile: {}
        };

        let atlasSections = this.getSections('sub_resource', 'type', 'TileSetAtlasSource');
        if(atlasSections.length === 0 || atlasSections.length <= atlasIndex) {
            throw "Atlas to save not found: " + atlasSections.length + " != " + atlasIndex;
        }

        let atlas = atlasSections[atlasIndex];
        for(let i = 0; i < atlas.content.length; i++) {
            let tileInfo = atlas.content[i][0].split('/');
            if(tileInfo.length < 3 || tileInfo[2].indexOf('physics_layer') < 0) {
                continue;
            }

            output.tile[atlas.content[i][0]] = atlas.content[i][1];
        }

        fs.writeFileSync(file, JSON.stringify(output, null, 2));
    }
}

module.exports = {
    create: function(file) {
        return new TilesetFile(file);
    }
};