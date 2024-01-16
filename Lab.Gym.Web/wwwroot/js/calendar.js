var calendar;
const momentFormat = 'DD/MM/YYYY HH:mm:ss'; //"DD/MM/YYYY h:mm A"
var isManager = false;
let currentEvent;
const formatDate = date => date === null ? '' : moment(date).format(momentFormat);
var fpStartTime;
var fpEndTime;

document.addEventListener('DOMContentLoaded', function () {

    isManager = $('.isManager').val() === 'True';

    if (isManager) {
        fpStartTime = flatpickr("#StartTime", {
            enableTime: true,
            dateFormat: "d/m/Y h:i K"
        });

        fpEndTime = flatpickr("#EndTime", {
            enableTime: true,
            dateFormat: "d/m/Y h:i K"
        });
    }
    
    var calendarEl = document.getElementById('calendar');
    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'timeGridWeek',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        dayHeaderFormat: function (objectCallback) {
            return moment(objectCallback.date).format('DD/MM');
        },
        eventClick: updateEvent,
        editable: false,
        selectable: isManager,
        select: (isManager ? addEvent : null),
        eventSourceFailure(error) {
            if (error) {
                console.log('Request to failed: ' + error);
            }
        },
        //events: {
        //    url: '/Schedule/Index?handler=CalendarEvents',
        //    method: 'GET',
        //    contentType: 'Content-type: application/json',
        //    dataType: "json",
        //    failure: function (error) {
        //        console.log('There was an error while fetching events: ' + error);
        //    },
        //    success: function (msg) {
        //        console.log('On get events: ');
        //        console.log(msg);
        //    }
        //}
        events: function (fetchInfo, successCallback, failureCallback) {

            var startFormat = moment(fetchInfo.start).format('YYYY-MM-DDTHH:mm:ssZ');
            var endFormat = moment(fetchInfo.end).format('YYYY-MM-DDTHH:mm:ssZ');
            $.ajax({
                type: "GET",    //WebMethods will not allow GET
                //url: "/Schedule/Index?handler=CalendarEvents?start=" + startFormat + "&end=" + endFormat,
                url: "/Schedule/Index?handler=CalendarEvents",
                data: {
                    start: startFormat,
                    end: endFormat
                },
                //completely take out 'data:' line if you don't want to pass to webmethod - Important to also change webmethod to not accept any parameters
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (doc) {
                    var events = [];   //javascript event object created here
                    var obj = doc;
                    $(obj).each(function () {
                        events.push({
                            title: $(this).attr('title'),  //your calevent object has identical parameters 'title', 'start', ect, so this will work
                            start: moment(moment($(this).attr('start'), momentFormat)).toDate(), // will be parsed into DateTime object
                            end: moment(moment($(this).attr('end'), momentFormat)).toDate(),
                            id: $(this).attr('id'),
                            description: $(this).attr('description'),
                            allDay: $(this).attr('allDay')
                        });
                    });
                    if (successCallback) {
                        successCallback(events);
                    }
                }
            });
        }
    });
    
    calendar.render();

    $('.btnCloseEventModal').on('click', () => {
        $('#eventModal').modal('hide');
    });

    $('.btnCloseEventModalTop').on('click', () => {
        $('#eventModal').modal('hide');
    });
});

/**
 * Calendar Methods
 **/

function updateEvent(item, element) {

    currentEvent = item.event;
    if ($(this).data("qtip")) $(this).qtip("hide");

    if (isManager) {
        $('#eventModalLabel').html('Edit event');
        $('#eventModalSave').html('Save');
    } else {
        $('#eventModalLabel').html(currentEvent.title);
    }

    $('#EventTitle').val(currentEvent.title);
    $('#Description').val(currentEvent.extendedProps.description);
    $('#isNewEvent').val(false);

    const start = formatDate(currentEvent.start);
    const end = formatDate(currentEvent.end);

    if (isManager) {
        fpStartTime.setDate(start);
        fpEndTime.setDate(end);

    } else {
        $('#StartTime').val(start);
        $('#EndTime').val(end);

    }

    if (currentEvent.allDay) {
        $('#AllDay').prop('checked', 'checked');
    } else {
        $('#AllDay')[0].checked = false;
    }

    $('#eventModal').modal('show');
}

function addEvent(event) {
    
    $('#eventForm')[0].reset();

    $('#eventModalLabel').html('Add new event');
    $('#eventModalSave').html('Save');
    $('#isNewEvent').val(true);

    fpStartTime.setDate(formatDate(event.start));
    fpEndTime.setDate(formatDate(event.end));

    $('#eventModal').modal('show');
}

/**
 * Modal
 * */

$('#eventModalSave').click(() => {
    
    const title = $('#EventTitle').val();
    const description = $('#Description').val();
    const startTime = moment($('#StartTime').val(), momentFormat);
    const endTime = moment($('#EndTime').val(), momentFormat);
    const isAllDay = $('#AllDay').is(":checked");
    const isNewEvent = $('#isNewEvent').val() === 'true' ? true : false;

    if (!title) {
        toastr.warning('Please enter a Title');
        return;
    }

    if (startTime > endTime) {

        toastr.warning('Start Time cannot be greater than End Time');
        return;
    } else if ((!startTime.isValid() || !endTime.isValid()) && !isAllDay) {

        toastr.warning('Please enter both Start Time and End Time');
        return;
    }

    const event = {
        title,
        description,
        isAllDay,
        startTime: startTime._i,
        endTime: endTime._i
    };

    if (isNewEvent) {
        sendAddEvent(event);
    } else {
        sendUpdateEvent(event);
    }
});

function sendAddEvent(event) {
    axios({
        method: 'post',
        url: '/Schedule/Index?handler=Event',
        headers: {
            'XSRF-TOKEN': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        contentType: "application/json",
        dataType: "json",
        data: {
            "Title": event.title,
            "Description": event.description,
            "Start": event.startTime,
            "End": event.endTime,
            "AllDay": event.isAllDay
        }
    })
    .then(res => {
        const { message, Id } = res.data;

        if (message === '') {
            const newEvent = {
                start: event.startTime,
                end: event.endTime,
                allDay: event.isAllDay,
                title: event.title,
                description: event.description,
                Id: event.Id
            };

            calendar.refetchEvents();
            $('#eventModal').modal('hide');
        } else {
            alert(`Something went wrong: ${message}`);
        }
    })
    .catch(err => alert(`Something went wrong: ${err}`));
}

function sendUpdateEvent(event) {
    axios({
        method: 'post',
        url: '/Schedule/Index?handler=UpdateEvent',
        headers: {
            'XSRF-TOKEN': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        dataType: "json",
        data: {
            "Id": currentEvent.id,
            "Title": event.title,
            "Description": event.description,
            "Start": event.startTime,
            "End": event.endTime,
            "AllDay": event.isAllDay
        }
    })
    .then(res => {
        const { message } = res.data;

        if (message === '') {
            currentEvent.title = event.title;
            currentEvent.description = event.description;
            currentEvent.start = event.startTime;
            currentEvent.end = event.endTime;
            currentEvent.allDay = event.isAllDay;

            calendar.refetchEvents();
            $('#eventModal').modal('hide');
        } else {
            alert(`Something went wrong: ${message}`);
        }
    })
    .catch(err => alert(`Something went wrong: ${err}`));
}

$('#deleteEvent').click(() => {
    if (confirm(`Do you really want to delte "${currentEvent.title}" event?`)) {
        axios({
            method: 'post',
            url: '/Schedule/Index?handler=DeleteEvent',
            headers: {
                'XSRF-TOKEN': $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            dataType: "json",
            data: {
                "eventId": currentEvent.id
            }
        })
            .then(res => {
                const { message } = res.data;

                if (message === '') {
                    calendar.refetchEvents();
                    $('#eventModal').modal('hide');
                } else {
                    alert(`Something went wrong: ${message}`);
                }
            })
            .catch(err => alert(`Something went wrong: ${err}`));
    }
});

$('#AllDay').on('change', function (e) {
    if (e.target.checked) {
        $('#EndTime').val('');
        fpEndTime.clear();
        this.checked = true;
    } else {
        this.checked = false;
    }
});

$('#EndTime').on('change', () => {
    $('#AllDay')[0].checked = false;
});
