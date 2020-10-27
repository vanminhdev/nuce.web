mainModule.directive('numberOnlyInput', function () {
    return {
        restrict: 'EA',
        template: '<input name="{{inputName}}" ng-model="inputValue" type="text" class="sm-form-control required" placeholder="Ví dụ 0.1" aria-required="true" />',
        //template: ' <input name="{{inputName}}" ng-model="inputValue" ng-keypress="suggest();" type="text" class="sm-form-control required" placeholder="Ví dụ 0.1" aria-required="true">',
        scope: {
            someCtrlFn: '&callbackFn',
            inputValue: '=',
            inputName: '='
        },
        link: function (scope) {

            scope.$watch('inputValue', function (newValue, oldValue) {
                var arr = String(newValue).split("");
                if (arr.length === 0) return;
                if (arr.length === 1 && (arr[0] == '-' || arr[0] === '.')) return;
                if (arr.length === 2 && newValue === '-.') return;
                if (isNaN(newValue)) { // không phải là số
                    scope.inputValue = oldValue;
                } else {
                    if (!isNaN(oldValue)) {
                        scope.someCtrlFn({ inputBTCNumber: scope.inputValue });
                    }
                }
            });
        }
    };
});


mainModule.directive('biClick', function ($parse) {
    return {
        compile: function ($element, attr) {
            var handler = $parse(attr.biClick);
            return function (scope, element, attr) {
                element.on('click', function (event) {
                    scope.$apply(function () {
                        var promise = handler(scope, { $event: event });
                        if (promise && angular.isFunction(promise.finally)) {
                            element.attr('disabled', true);
                            promise.finally(function () {
                                element.attr('disabled', false);
                            });
                        }
                    });
                });
            };
        }
    };
});

// load lại kết quả bài khảo sát của người dùng 
mainModule.directive("pageAttemptting", ['$timeout', function ($timeout) {
    return function (scope, element, attrs) {
        scope.$watch("testContent", function (value) {
            $timeout(function () {
                if (scope.saveUserAnswers != null && scope.saveUserAnswers != "" && scope.saveUserAnswers.userAnswers != null) {
                    angular.forEach(scope.saveUserAnswers.userAnswers, function (a) {
                        if (a.ty == 'SC' || a.ty == 'MC') {
                            // đánh dấu checkbox câu trả lời 
                            angular.forEach(a.a, function (at) {
                                // tickId = sectionId + '-' + questionid + '-' + answerId
                                var tickId = 'answer-tick-' + a.s;
                                tickId = tickId + '-' + a.q;
                                tickId = tickId + '-' + at;
                                //console.log($('#' + tickId));
                                $('#' + tickId).attr('checked', true);
                                //$('#answer-tick-15-16-46').attr('checked', true);
                                //document.getElementById(tickId).checked = true;
                                //$('#' + tickId).attr('checked', "checked");
                            });
                        } else if (a.ty == 'SHORT') {
                            var answerTextareaId = $('#answer-text-' + a.s + '-' + a.q);
                            $(answerTextareaId).val(a.te);
                        }
                    });

                    //var count = 0;
                    //$('.question-title').each(function (i, obj) {
                    //    count++;
                    //    var id = '#' + $(this).attr("id");
                    //    if ($(this).find('span.quesIndex').length == 0)
                    //    {
                    //        $("<span class='quesIndex'>" + count + "</span>").insertBefore($(this));
                    //    }
                    //});

                }

                $(".attempt-disable-answer").prop('disabled', true);
            });

        });
    };
}]);


// prevent double submiting 
//http://jsfiddle.net/miensol/7PETf/3/
//http://miensol.pl/angularjs/2014/05/21/ngclick-and-double-post.html