namespace nuce.web.thi
{
    public class LoaiCauHoi
    {
        private int m_Id;
        private string m_Name;
        private string m_Description;

        public int ID
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #region Constructor

        //public LoaiCauHoi(int Id)
        //{

        //}
        #endregion
    }
}