var SingleOptionId = ["选项A", "选项B", "选项C", "选项D"]
var MultipleOptionId = ["选项A", "选项B", "选项C", "选项D", "选项E", "选项F", "选项G", "选项H", "选项I", "选项J", "选项K", "选项L", "选项M", "选项N", "选项O", "选项P", "选项Q", "选项R", "选项S", "选项T", "选项U", "选项V", "选项W", "选项X", "选项Y", "选项Z"];
$(document).ready(function () {
    //添加判断题
    $('#createtq').click(function addTrueOrFalse() {
        var tqItem = $('#tq0').clone();
        var tqSum = $('.trueorfalse-question').size();
        tqItem.attr('id', 'tq' + tqSum);
        tqItem.find('label').eq(0).attr('for', 'TrueOrFalseQuestions_' + tqSum + '__Content');
        tqItem.find('textarea').attr('name', 'TrueOrFalseQuestions[' + tqSum + '].Content').attr('id', 'TrueOrFalseQuestions_' + tqSum + '__Content').val("");
        tqItem.find('span').eq(0).attr('data-valmsg-for', 'TrueOrFalseQuestions[' + tqSum + '].Content');
        tqItem.find('label').eq(1).attr('for', 'TrueOrFalseQuestions_' + tqSum + '__IsCorrect');

        tqItem.find('input').eq(0).attr('name', 'TrueOrFalseQuestions[' + tqSum + '].IsCorrect').attr('id', 'TrueOrFalseQuestions_' + tqSum + '__IsCorrect');
        tqItem.find('input').eq(1).attr('name', 'TrueOrFalseQuestions[' + tqSum + '].IsCorrect')

        tqItem.find('span').eq(1).attr('data-valmsg-for', 'TrueOrFalseQuestions[' + tqSum + '].IsCorrect')
        $('#trueorfalse-question-createpanel').before(tqItem);
    });
            
    //添加单选题
    $('#createsq').click(function addSingle() {
        var sqItem = $('#sq0').clone();
        var sqSum = $('.single-question').size();
        sqItem.attr("id", 'sq' + sqSum);

        sqItem.find('label').eq(0).attr('for', 'SingleQuestions_' + sqSum + '__Content');
        sqItem.find('textarea').eq(0).attr('name', 'SingleQuestions[' + sqSum + '].Content').attr('id', 'SingleQuestions_' + sqSum + '__Content').val("");
        sqItem.find('span').eq(0).attr('data-valmsg-for', 'SingleQuestions[' + sqSum + '].Content');

        for (var item in sqItem.children('.single-question-option')) {
            var opNum = 0;
            $(this).find('textarea').attr('id', 'SingleQuestions_' + sqSum + '__Options_' + opNum + '__OptionProperty').attr('name', 'SingleQuestions[' + sqSum + '].Options[' + opNum + '].OptionProperty').attr('placeholder', '在这里输入' + SingleOptionId[opNum] + '内容').val("");
            $(this).find('span').attr('data-valmsg-for', 'SingleQuestions[' + sqSum + '].Options[' + opNum + '].OptionProperty');
            $(this).find('input').attr('id', 'SingleQuestions_' + sqSum + '__CorrectAnswer').attr('name', 'SingleQuestions[' + sqSum + '].CorrectAnswer');
            opNum++;
        }
        $('#single-question-createpanel').before(sqItem);
    })

    //添加多选题选项
    $('.multiple-option-createbutton').click(function addMultipleOption() {
        var opSum = $(this).closest('.multiple-question').children('.multiple-question-option').size();    //得到当前多选题选项的数目
        if (opSum > 24) { alert("错误，不能超过24个选项！") }
        else
        {
            var mqposition = $(this).closest('.multiple-question').attr('id').match(/\d+/)[0];    //得到当前多选题的id，因为得到的是字符串，所以要用[0]得到数字
            var moitem = $('#mq0').children('.multiple-question-option').first().clone();
            moitem.children('label').text(MultipleOptionId[opSum]);

            moitem.find('textarea').attr('name', 'MultipleQuestions[' + mqposition + '].Options[' + opSum + '].OptionProperty').attr('id', 'MultipleQuestions_' + mqposition + '__Options_' + opSum + '__OptionProperty').attr('placeholder', '在这里输入' + MultipleOptionId[opSum] + '内容').val("");
            moitem.find('span').eq(0).attr('data-valmsg-for', 'MultipleQuestions[' + mqposition + '].Options[' + opSum + '].OptionProperty');

            moitem.find('input').attr('name', 'MultipleQuestions[' + mqposition + '].Options[' + opSum + '].IsCorrect');
            moitem.find('input').eq(0).attr('id', 'MultipleQuestions_' + mqposition + '__Options_' + opSum + '__IsCorrect');
            moitem.find('span').eq(1).attr('data-valmsg-for', 'MultipleQuestions[' + mqposition + '].Options[' + opSum + '].IsCorrect');
            $('#mq' + mqposition).children('.multiple-question-option').last().after(moitem);
        }
    })

    //添加多选题
    $("#createmq").click(function addMultiple() {
        var mqContentItem = $('#mq0').children('.multiple-question-content').clone();
        var mqOptionItem = $('#mq0').children('.multiple-question-option').first().clone();
        var mqPanelItem = $('#mq0').children('.multiple-option-createpanel').clone(true);

        var mqSum = $(".multiple-question").size();     //得到目前所有多选题的数目

        var container = $('<div></div>').addClass('multiple-question').attr('id', 'mq' + mqSum);

        mqContentItem.children('label').attr('for', 'MultipleQuestions_' + mqSum + '__Content');
        mqContentItem.find('textarea').attr('name', 'MultipleQuestions[' + mqSum + '].Content').attr('id', 'MultipleQuestions_' + mqSum + '__Content').val("");
        mqContentItem.find('span').attr('data-valmsg-for', 'MultipleQuestions[' + mqSum + '].Content');

        mqOptionItem.find('textarea').attr('name', 'MultipleQuestions[' + mqSum + '].Options[0].OptionProperty').attr('id', 'MultipleQuestions_' + mqSum + '__Options_0__OptionProperty');
        mqOptionItem.find('span').eq(0).attr('data-valmsg-for', 'MultipleQuestions[' + mqSum + '].Options[0].OptionProperty');
        mqOptionItem.find('input').attr('name', 'MultipleQuestions[' + mqSum + '].Options[0].IsCorrect');
        mqOptionItem.find('input').eq(0).attr('id', 'MultipleQuestions_' + mqSum + '__Options_0__IsCorrect');
        mqOptionItem.find('span').eq(1).attr('data-valmsg-for', 'MultipleQuestions[' + mqSum + '].Options[0].IsCorrect');

        var finalobject = container.append(mqContentItem).append(mqOptionItem).append(mqPanelItem);

        $('#multiple-question-createpanel').before(finalobject);
    })
})