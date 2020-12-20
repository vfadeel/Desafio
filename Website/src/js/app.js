angular.module("app", ['ngRoute', 'ngMask', 'rw.moneymask'])

.config(function($routeProvider){

    $routeProvider.when("/titulo", {
        templateUrl:"src/templates/titulo.html",
        controller:"tituloController"
    })
    .otherwise({redirectTo:"/titulo"});

});