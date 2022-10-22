using System;
using System.Collections.Generic;

// Creates the columns and buttons
namespace Commercial_Controller
{
    public class Battery
    {
        public int _ID, _amountOfFloors, _amountOfColumns, _amountOfBasements, _columnID, _amountOfElevatorPerColumn ;
        public string status;
        public List<Column> columnsList;
        public List<FloorRequestButton> _floorRequestButtonList;

        public int columnID = 1;
        public int elevatorID = 1;
        public int floorRequestButtonID = 1;
        public int callButtonID = 1;
        public int floor;

        public int servedFloors;
        
        public Battery(int _ID, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
           this._ID = _ID;
           this.status = "online";
           this._amountOfFloors = _amountOfFloors;
           this._amountOfBasements = _amountOfBasements;
           this._amountOfElevatorPerColumn = _amountOfElevatorPerColumn;
           this.columnsList = new List<Column>();
           this._floorRequestButtonList = new List<FloorRequestButton>();
           this._columnID = 1;
           

           if (this._amountOfBasements > 0) {
            createBasementFloorRequestButtons(_amountOfBasements);
            createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
            _amountOfColumns--;
           }

           this.createBasementFloorRequestButtons(_amountOfFloors);
           this.createColumns(_amountOfColumns, _amountOfFloors, _amountOfElevatorPerColumn);
        


        }
        // Create the column for the basement and add to the servedFLoors list
        public void createBasementColumn(int _amountOfBasements,int _amountOfElevatorPerColumn) {
            List<int> servedFloors = new List<int>();
            int floor = -1;
            for (int i = 0; i < _amountOfBasements; i++) {
                servedFloors.Add(floor);
                floor--;

            }

            Column column = new Column(columnID, "online", _amountOfElevatorPerColumn, servedFloors, true);
            columnsList.Add(column);
            _columnID++;


        }
        // Create columns and add to the servedFloors list
        public void createColumns (int _amountOfColumns, int _amountOfFloors, int _amountOfElevatorPerColumn) {
            int amountOfFloorsPerColumn = (int)Math.Round((double)_amountOfFloors / _amountOfColumns);
            int floor = 1;
            
            for (int i = 0; i < amountOfFloorsPerColumn; i++) {
                List<int> _servedFloors = new List<int>();
                for(int j = 0; j < amountOfFloorsPerColumn; j++) {
                    if (floor <= _amountOfBasements){
                        _servedFloors.Add(floor);
                        floor++;

                    }

                    Column column = new Column(columnID, "online", _amountOfElevatorPerColumn, _servedFloors, false);
                    columnsList.Add(column);
                    _columnID++;
                }
            }
        }
        // Adds the requests of people on floor levels to the floorRequestsButtonsList list
        public void createFloorRequestButtons(int _amountOfFloors) 
        {
            int buttonFloor = 1;
            for (int i = 0; i < _amountOfFloors; i++) 
            {
               
                FloorRequestButton floorRequestButton =  new FloorRequestButton(floorRequestButtonID, "OFF", buttonFloor, "up");
                this._floorRequestButtonList.Add(floorRequestButton);
                buttonFloor++;
                floorRequestButtonID++;
            }
        }
        
        // Adds the requests of people at the basement into the floorRequestsButtonsList list
        public void createBasementFloorRequestButtons(int _amountOfBasements) {
            int buttonFloor = 1;
            for (int i = 0; i < _amountOfBasements; i++) {
               FloorRequestButton floorRequestButton = new FloorRequestButton(floorRequestButtonID, "OFF", buttonFloor, "down");
               this._floorRequestButtonList.Add(floorRequestButton);
               buttonFloor--;
               floorRequestButtonID++;

            }

        }
        
        // Select the best available column for the scenario 
        public Column findBestColumn(int _requestedFloor){
            foreach (Column column in this.columnsList) {
                if (column.servedFloors.Contains(_requestedFloor)) {
                  return column;  
                    
                }
            }
            return null;
        }

        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {   
            
            Column column = findBestColumn(_requestedFloor);
            Elevator elevator = column.findElevator(1, _direction);
            elevator.addNewRequest(1);
            elevator.move();

            elevator.addNewRequest(_requestedFloor);
            elevator.move();

            return(column, elevator);
            
        }
    }
}

