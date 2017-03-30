$('document').ready(function(){
	var new_course = {};
	var all_course = {};
	var course_count = 0;
	var course_section = document.getElementById("add-course-section");
	var event = {};

	function createNewEventPanel(event){
		if (event.important == true){
				course_section.innerHTML += "<li class='event_list' id='event'" + course_count + ">" +
				"<div class='panel panel-info'>"+
					"<div class='panel-heading'>"+
						"<h3 class='panel-title'>" + event.name + "[important]" +  "</h3>"+
					"</div>"+
					"<div class='panel-body'> "+
					"<ul>"+
						"<p>Course "+ event.name + 
						"</p>"+
						"<p>"+ event.comment +
						"<div class='assignemntActionButtonOutterWrapper'>\
							<button class='modal-body btn btn-primary left-block btn-danger' data-toggle='modal' data-target='#delete-course'>edit</button></li>\
							<button class='modal-body btn btn-primary left-block btn-danger' data-toggle='modal' data-target='#delete-course'>delete</button></li>\
						</div>" +
						 "</p>"+
						"</ul>"+
					"</div>"+
					"</div>"+
				" </li>";

			} 
		else{
			course_section.innerHTML += "<li id='event'" + course_count + ">" +
			"<div class='panel panel-info'>"+
				"<div class='panel-heading'>"+
					"<h3 class='panel-title'>" + event.name +  "</h3>"+
				"</div>"+
				"<div class='panel-body'> "+
				"<ul>"+
					"<p>"+ event.comment + "</p>"+
					"</ul>"+
				"</div>"+
				"</div>"+
			" </li>";
		}

			course_count++;
	}
		
		function validateForm(){

			event = createNewEvent();

			if (event.name == "" || event.type == null){
				return false;
			}
			return true;

		}
		
		function createNewEvent(){

			var newEvent= {};
			
			newEvent.name = $("#name-add-course").val();
			if ($("#important-add-course").val() == "on"){
				newEvent.important = true;
			} else {
				newEvent.important = false;
			}
			
			newEvent.comment = $("#comment-add-course").val();
			newEvent.type = $("#event-type-slection").val();
			
			return newEvent;
		}
		
		$("#add-course-button").on("click", function(){
			var valid = validateForm();

			if (valid){
				$("#add-course").modal('toggle');
				var new_event = createNewEvent();
				createNewEventPanel(new_event);
				all_course.push(event);   // record the present event
				
			}else{
				;
			}
				
		});
	
});