var Attendee = function (data) {
    var self = this;
    self.name = data.name;
};

var PageViewModel = function () {
    var self = this;

    self.title = ko.observable("Xº Reunião NetPonto - Y");
    self.attendees = ko.observableArray([]);
    self.winners = ko.observableArray([]);
    self.prizes = ko.observableArray([]);
    self.started = ko.observable(false);
    self.version = ko.observable("");

    $.get("version").done(function(content){
        self.version(content);
    })

    var setup_localstorage_backing = function(fieldName) {
        try{
            var keyName = "sorteio_"+fieldName;
            if(localStorage[keyName]){
                self[fieldName](JSON.parse(localStorage[keyName]));
            }
        }catch(e){
            console.error("Error loading value from localstorage for "+keyName);
            console.error(e);
        }

        self[fieldName].subscribe(function(val) { localStorage[keyName] = JSON.stringify(val); });
    };

    ["title","attendees","winners","prizes"].forEach(setup_localstorage_backing);

    self.nuke_storage = function (){
        localStorage.clear();
    }

    self.load_file = function (selected_file) {

        window.netponto_ns.selected_file = selected_file;

        var reader = new FileReader();

        reader.addEventListener("loadend", function () {
            var attendees = reader.result.split("\n").forEach(function (line) {
                var name = line.trim();
                if (name === "")
                    return;
                var attendee = new Attendee({
                    name: line.trim()
                });
                self.attendees.push(attendee);
            });
        });

        if (typeof (selected_file) !== "undefined" && selected_file !== null) {
            reader.readAsText(selected_file);
        }
    };

    self.add_attendee = function () {
        var name = window.prompt('Nome?');
        if (name === "" || name === null) {
            return;
        }
        self.attendees.push(new Attendee({ name: name.trim() }));
    };

    self.remove_attendee = function (attendee) {
        self.attendees.remove(attendee);
    };

    self.clear_attendees = function () {
        if (window.confirm('Tem a certeza que pretende limpar a lista?')) {
            self.attendees.removeAll();
        }
    };

    self.start = function () {
        self.started(true);
        StartCloudTags();
    };

    self.rollIt = function () {
        RollIt();
    };
};

window.onerror = function (errorMsg, url, lineNumber) {
    // for unexpected errors only
    alert('Error: ' + errorMsg + '\n\nScript: ' + url + '\nLine: ' + lineNumber);
};

var canRoll = true;
var o = {
    textFont: 'Arial, Helvetica, sans-serif',
    maxSpeed: 0.05,
    minSpeed: 0.01,
    textColour: '#039',
    textHeight: 25,
    outlineMethod: 'colour',
    fadeIn: 800,
    outlineColour: '#900',
    outlineOffset: 0,
    depth: 0.97,
    minBrightness: 0.2,
    wheelZoom: false,
    reverse: true,
    shadowBlur: 2,
    shuffleTags: true,
    shadowOffset: [1, 1],
    stretchX: 1.7,
    initial: [0, 0.1],
    clickToFront: 600,
    noMouse: true
};

function StartCloudTags() {
    var s = (new Date).getTime() / 360;
    o.initial[0] = 0.2 * Math.cos(s);
    o.initial[1] = 0.2 * Math.sin(s);
    TagCanvas.Start('canvas', 'tags', o);
}

function RollIt() {
    if (canRoll) {
        // Check if modal is open
        var isOpen = $("#winnersModal").hasClass('in');

        if (isOpen) {
            $("#winnersModal").modal('toggle');
        }

        canRoll = false;
        GetItem(Math.floor(getRandomArbitrary(5, 10)));
    }
}

function GetItem(index) {
    if (index === 0) {
        console.log("TagToFront"),
        TagToFront();
        return;
    }

    --index;

    var ms = getRandomArbitrary(500, 1500);
    setTimeout(function () {
        Rotate();
        GetItem(index);
    }, ms);
}

function getRandomArbitrary(min, max) {
    return Math.random() * (max - min) + min;
}

function Rotate() {
    TagCanvas.RotateTag('canvas', {
        index: Math.floor(Math.random() * 20), lat: -60, lng: -60, time: 800, active: 1
    });
}

function TagToFront() {
    TagCanvas.TagToFront('canvas', {
        index: Math.floor(Math.random() * $("#tags").children().length),
        active: 1,
        callback: result
    });
}

function result(e, item) {
    var value = item.text_original;

    $("#tags").children('a').each(function () {
        if (value === this.text) {
            $(this).remove();
            $("#winnerName").text(value);
            $("#winnersModal").modal('toggle');
            viewModel.winners.push(value);
            setTimeout(function () {
                TagCanvas.Update('canvas');
                canRoll = true;
            }, 1000);
        }
    });
}
