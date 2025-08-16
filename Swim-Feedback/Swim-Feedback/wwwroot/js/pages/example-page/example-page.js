var examplePage = {};

examplePage.init = function(examplePageRef) {
    examplePage.examplePageRef = examplePageRef;

    examplePage.jsToCS();
};

examplePage.jsToCS = function() {
    examplePage.examplePageRef.invokeMethodAsync("JSToCS", "This is JS");
};

examplePage.csToJS = function(text) {
    console.log(text);
};