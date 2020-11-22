// Musicxml(Muse(xml)) to JSON in External then JSON to Notes 

var fs = require('fs');

function getNotes(data, callback) {
    var notes = [];

    var d = JSON.parse(data);
    var partwise = d["score-partwise"]
    var measures = partwise.part.measure;
    
    for (let i = 0; i < measures.length; i++) {
        const currentMeasure = measures[i].note;
        for (let j = 0; j < currentMeasure.length; j++) {
            const currentNote = currentMeasure[j];
            if (!currentNote.pitch || !currentNote.duration || !currentNote.voice) {
                continue;
            }
            notes.push(
                {
                   "step":  currentNote.pitch.step._text,
                   "octave": currentNote.pitch.octave._text, 
                   "duration": currentNote.duration._text,
                   "voice": currentNote.voice._text
                }
            );
        }
    }
    console.log(notes.length);
    callback(notes)
}

console.log("Started");
fs.readFile("./Beethoven-MoonlightSonata-Pure.json", function (err, data) {
    if (err) {
        console.error(err);
        return;
    }
    getNotes(data, function (content) {
        fs.writeFile('./Beethoven-MoonlightSonata.json', JSON.stringify(content, null, 4), error => {
            if (error) {
                console.error(error);
                return;
            }
            console.log("write complete");
        })
    });
});


