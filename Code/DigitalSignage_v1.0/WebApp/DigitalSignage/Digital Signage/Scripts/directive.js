(function (window, angular, undefined) {
  'use strict';

  angular.module('TestApp')
  .directive('tmpl', testComp);
  
function testComp($compile){
  console.log('sss');
    var directive = {};
	
    directive.restrict = 'A';
    directive.templateUrl = 'app/view/_child.html'; 
    directive.transclude = true;   
    directive.link = function(scope, element, attrs){
      
    }
    return directive;
  }

})(window, window.angular);
