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
		
			/*
		<p id="profile_name">Name: Yuan Feng</p>
					<p id="profile_password">Password: Yuan Feng</p>
					<p id="profile_street">Address: 1101 Bay Street</p>
					<p id="profile_name">Hobby: Web Programming</p>
		*/
		
		$("#submit_profile_info").on("click", function(){
			var new_name = document.getElementById("name-edit-name").value;
			var new_password = document.getElementById("name-edit-password").value;
			var new_address = document.getElementById("name-edit-address").value;	
			var new_hobby = document.getElementById("name-edit-hobby").value;
			
			//window.alert(new_hobby);
			var str = "";
			var password_locator = document.getElementById('profile_name');
			str = "Name: " + new_name;
			password_locator.innerHTML = str;

			var password_locator = document.getElementById('profile_password');
			str = "Password: " + new_password;
			password_locator.innerHTML = str;
			
			var address_locator = document.getElementById('profile_address');
			str = "Address: " + new_address;
			address_locator.innerHTML = str;
			
			var hobby_locator = document.getElementById('profile_hobby');
			str = "Hobby: " + new_hobby;
			hobby_locator.innerHTML = str;
			
		});
	
});