var calendar;
document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'timeGridWeek',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        eventClick: updateEvent,
        selectable: true,
        select: addEvent,
        eventSourceFailure(error) {
            if (error) {
                console.log('Request to failed: ' + error);
            }
        },
        events: {
            url: '/Schedule/Index?handler=CalendarEvents',
            method: 'GET',
            contentType: 'Content-type: application/json',
            dataType: "json",
            failure: function (error) {
                console.log('There was an error while fetching events: ' + error);
            }
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

let currentEvent;
const formatDate = date => date === null ? '' : moment(date).format("MM/DD/YYYY h:mm A");
const fpStartTime = flatpickr("#StartTime", {
    enableTime: true,
    dateFormat: "m/d/Y h:i K"
});
const fpEndTime = flatpickr("#EndTime", {
    enableTime: true,
    dateFormat: "m/d/Y h:i K"
});

/**
 * Calendar Methods
 **/

function updateEvent(item, element) {
    currentEvent = item.event;

    if ($(this).data("qtip")) $(this).qtip("hide");

    $('#eventModalLabel').html('Edit Event');
    $('#eventModalSave').html('Update Event');
    $('#EventTitle').val(currentEvent.title);
    $('#Description').val(currentEvent.description);
    $('#isNewEvent').val(false);

    const start = formatDate(currentEvent.start);
    const end = formatDate(currentEvent.end);

    fpStartTime.setDate(start);
    fpEndTime.setDate(end);

    $('#StartTime').val(start);
    $('#EndTime').val(end);

    if (currentEvent.allDay) {
        $('#AllDay').prop('checked', 'checked');
    } else {
        $('#AllDay')[0].checked = false;
    }

    $('#eventModal').modal('show');
}

function addEvent(event) {
    
    $('#eventForm')[0].reset();

    $('#eventModalLabel').html('Add Event');
    $('#eventModalSave').html('Create Event');
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
    const startTime = moment($('#StartTime').val());
    const endTime = moment($('#EndTime').val());
    const isAllDay = $('#AllDay').is(":checked");
    const isNewEvent = $('#isNewEvent').val() === 'true' ? true : false;

    if (!title) {
        alert('Please enter a Title');
        return;
    }

    if (startTime > endTime) {

        alert('Start Time cannot be greater than End Time');
        return;
    } else if ((!startTime.isValid() || !endTime.isValid()) && !isAllDay) {

        alert('Please enter both Start Time and End Time');
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
