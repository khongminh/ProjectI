$(document).ready(function () {
  var connection = new signalR.HubConnectionBuilder().withUrl("/enrollment").build();

  connection.start().catch(function (err) {
    return console.error(err.toString());
  });

  connection.on("Increase", function (classIds) {
    $('tbody tr').each(function (index, element) {
      var classId = $(element).find('td').eq(0).text();

      classIds.forEach(function (item) {
        if (Number(classId) == item) {
          var currentCount = $(element).find('td').eq(7).text();
          $(element).find('td').eq(7).text(Number(currentCount) + 1);
        }
      });
    });
    $(':checkbox').prop("checked", false);
    $.notify("Đăng ký thành công");
  });


  $('#send').on("click", function () {
    var isTrue = true;
    var enrollments = [];
    var studentId = $('table').data('studentId');

    $('tbody tr').each(function (index, element) {
      var classChecks = $(element).find('td').eq(8).find('input');
      if (classChecks.is(":checked")) {

        var maxStudent = $(element).find('td').eq(6).text();
        var enrollmentStudent = $(element).find('td').eq(7).text();
        if (maxStudent === enrollmentStudent) {
          console.log('Hello');
          isTrue = false;
        }
        var obj = new Object();
        obj.ClassroomId = Number($(element).find('td').eq(0).text());
        obj.StudentId = studentId;
        enrollments.push(obj);
      }
    });
    if (isTrue) {
      connection.invoke("Enrollment", enrollments).catch(function (err) {
        return console.error(err.toString());
      });
    }
    else {
      $.notify("Lớp đã đầy. Vui lòng chọn lớp khác");
      $(':checkbox').prop("checked", false);
    }

    event.preventDefault();
  });
});

