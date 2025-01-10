# PlaneTracker
Projects simulates an app tracking flights. Flights are displayed as plane icons on a world map. Whole of the project is implemented using lots of different design patterns

## Initialization:
Data for all objects should be provided in the .ftr format. Objects can be initialized directly from file at once or in chunks which come to the program in time intervals.

## Data persistance:
The "print" command saves in an actual snapshot of the program in a .json file called snapshot_HH_MM_SS.json.
The command "exit" saves the data and closes the app.

## Features:
The "report" command shows info on the state of some objects provided by several "medias" included in the program

## Data manipulation:
User can manipulate data by inserting SQL-like commands into the command line  
**displaying objects:**  
dispay {object fields of * for all object fields} from {object class} [where {conditions}]  
example: display ID, TakeoffTime, WorldPosition from Flights where ID > 10 and ID < 20  
**updating objects:**
update {object class} set ({key_value_list}) [where {conditions}]  
example: update Flights set (WorldPosition.Lat=54.5323, WorldPosition.Long=21.453) where ID=34 or ID=50  
**deleting objects:**  
delete {object class} [where {conditions}]  
example: delete Flights where WorldPosition.Lat > 45.0 or WorldPosition.Long < -70.0  
**adding objects:**  
add {object_class} new ({key_value_list})  
example: add Flights new (ID=123, WorldPosition.Lat=21.0, WorldPosition.Long=50.0)
