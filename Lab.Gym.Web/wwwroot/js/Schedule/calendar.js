var calendar;
const momentFormat = 'DD/MM/YYYY HH:mm:ss'; //"DD/MM/YYYY h:mm A"
const datePickerFormat = "d/m/Y H:i";
const datePickerAriaDateFormat = "d/m/Y H:i";
var isManager = false;
let currentEventId;
var fpStartTime;
var fpEndTime;
const formatDate = date => date === null ? '' : moment(date).format(momentFormat);

document.addEventListener('DOMContentLoaded', function () {

    isManager = $('.isManager').val() === 'True';

    if (isManager) {
        fpStartTime = flatpickr("#StartTime", {
            enableTime: true,
            ariaDateFormat: datePickerAriaDateFormat,
            dateFormat: datePickerFormat
        });

        fpEndTime = flatpickr("#EndTime", {
            enableTime: true,
            ariaDateFormat: datePickerAriaDateFormat,
            dateFormat: datePickerFormat
        });
    }

    calendar = new FullCalendar.Calendar(document.getElementById('calendar'), {
        initialView: 'timeGridWeek',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        dayHeaderFormat: function (objectCallback) {
            return moment(objectCallback.date).format('DD/MM');
        },
        eventClick: onCalendarUpdateEventClick,
        editable: false,
        selectable: isManager,
        select: (isManager ? onCalendarAddNewEvent : null),
        eventSourceFailure(error) {
            if (error) {
                console.log('Request to failed: ' + error);
            }
        },
        events: getCalendarEvents
    });
    
    calendar.render();
});

function onCalendarUpdateEventClick(item, element) {

    const currentEvent = item.event;
    currentEventId = currentEvent.id;
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

function onCalendarAddNewEvent(event) {
    
    $('#eventForm')[0].reset();
    $('#eventModalLabel').html('Add new event');
    $('#eventModalSave').html('Save');
    $('#isNewEvent').val(true);

    fpStartTime.setDate(formatDate(event.start));
    fpEndTime.setDate(formatDate(event.end));

    $('#eventModal').modal('show');
}


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
        sendUpdateEvent(currentEventId, event);
    }
});

$('#deleteEvent').click(() => {
    $('.modalConfirmDeleteEventTitle').text($('#EventTitle').val());
    $('#confirmDeleteModal').modal('show');
});

$('#modalConfirmDelete').click(() => {

    sendDeleteEvent(currentEventId);
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
