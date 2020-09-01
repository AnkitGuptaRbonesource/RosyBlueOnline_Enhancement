window.onbeforeunload = function () {
    try {
		device.release();
    }
    catch (err) {
       // return false;
    }
    //return false;
}

var visProp = getHiddenProp();
if (visProp) {
    var evtname = visProp.replace(/[H|h]idden/, '') + 'visibilitychange';
    document.addEventListener(evtname, visChange);
}

function visChange() {

    if (isHidden()) {
        try {
			device.release();
        }
        catch (err) {
            return false;
        }
    }
    else
        console.log("Tab Visible!");
}
function getHiddenProp() {
    var prefixes = ['webkit', 'moz', 'ms', 'o'];

    // if 'hidden' is natively supported just return it
    if ('hidden' in document) return 'hidden';

    // otherwise loop over all the known prefixes until we find one
    for (var i = 0; i < prefixes.length; i++) {
        if ((prefixes[i] + 'Hidden') in document)
            return prefixes[i] + 'Hidden';
    }

    // otherwise it's not supported
    return null;
}
function isHidden() {
    var prop = getHiddenProp();
    if (!prop) return false;

    return document[prop];
}

