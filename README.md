# CodeChallenge - Andy DiStasi Notes
Hope this is OK, I just wanted to add a little context around some of the technical decisions I made in implementation

## Task 1 Model Update
I updated the Employee model with a custom getter & setter for DirectReports.  This wouldn't be necessary if we were working with an actual database, but based on the way .NET's JSON Library serializes it's values, an Employee without DirectReports gets a value of null instead of an Empty List.  I've customized the getter to return an empty list if the value is null to make the application behave more predictably and reduce the need for additional logic elsewhere checking for null values.

## Task 1 Hierarchy Loading
Default EntityFramework behavior is that the query will not Eager Load deeper than the top level of direct reports.  There is an argument to be made for configuring the Query as described here (https://michaelceber.medium.com/implementing-a-recursive-projection-query-in-c-and-entity-framework-core-240945122be6) to allow for eager loading.  This would really only become a concern in a very deep hierarchy, as this approach would then introduce significantly more trips to the database.  For the sake of readability and given the size of this data, I felt additional trips to the database was acceptable

## Task 2 Controller Structure
I probably overthought this, but I went back and forth for a while on if it was appropriate to put the Compensation endpoints in the Employee Controller or create a separate controller for it.  I don't think there's an objectively right or wrong answer, but I opted to create a new controller for 2 reasons: more robust separations of concerns and to show that I knew how to do it for the purposes of this demo.  Condensing this back into the Employee Controller would be largely copy/paste with some simple route renaming.

## Task 2 Compensation Model Structure
I was a little unsure as to the desired structure of the Compensation object.  For a POST it didn't feel appropriate to upload an employee object (one of the listed fields in the task), so I added an EmployeeID attribute as well to accommodate passing that data through the body (mirroring the approach used for the Employee POST as closely as possible).  I also added a CompensationID attribute to serve as a Primary Key.  I use the EmployeeID to retrieve the Employee object and attach that to the Employee attribute of the Compensation model in the controller.
