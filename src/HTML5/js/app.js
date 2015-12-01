var app = {
    Prizes: [],
    Meeting: {
        Name: "",
        Local: "",
        toString: function () {
            return this.Name + " - " + this.Local;
        }
    },
    Users: [],
    setConfig: function (response) {
        this.Meeting.Name = response.event.name;
        this.Meeting.Local = response.event.local;

        this.Prizes = response.prizes.slice();
    },
    loadData: function (callback) {
        // Load all people

        // TODO replace this string in config.json key
        $.get("data/data.json", function (response) {

            response.data.sort();
            app.Users = response.data.slice();

            callback();
        });
    },
    getPrize: function () {
        return this.Prizes.pop();
    },
    addPrizeBack: function (prize) {
        this.Prizes.push(prize);
    }
};
