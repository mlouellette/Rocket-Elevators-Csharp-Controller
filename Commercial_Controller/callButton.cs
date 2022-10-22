namespace Commercial_Controller
{
    //Button on a floor or basement to go back to lobby
    public class CallButton
    {
        public int _ID, _floor;
        public string _direction, status;

        public CallButton(int _ID, string _status, int _floor, string _direction)
        {
            this._ID = _ID;
            this.status = _status;
            this._floor = _floor;
            this._direction = _direction;
            
            
        }
    }
}