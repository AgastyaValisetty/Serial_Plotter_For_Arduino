using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using ZedGraph;
namespace Test_3
{
    public partial class Form1 : Form
    {
        private double[] Xaxis = new double[30];
        GraphPane GraphPane;
        double _y= 0;
        int NoOfTrys = 0;
        int att = 0;

        bool CanCheck = true;

        public Form1()
        {
            
            InitializeComponent();
            GraphPane = zedGraphControl1.GraphPane;
            DrawSignCurve();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            string[] ports = SerialPort.GetPortNames();
            Comm.Items.AddRange(ports);
            Comm.SelectedIndex = 0;
            DisconnectBtn.Enabled = false;

            


            if (serialPort1.IsOpen && CanCheck == true)
            {
                try
                {
                    _y = serialPort1.ReadByte();
                }
                catch (Exception ex)
                {
                    throw;

                }
            }
            if (NoOfTrys > 5)
            {
                CanCheck = false;
            }
            if (serialPort1.IsOpen)
            {
                _y = serialPort1.ReadByte();

            }
            else
            {
                return;
            }
        }
        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            timer1.Start();
            Thread masterthread;
            masterthread = new Thread(runit);
            masterthread.Start();
            
            ConnectBtn.Enabled = false;
            DisconnectBtn.Enabled = true;
            
            try
            {
                serialPort1.PortName = Comm.Text;
                serialPort1.BaudRate = Convert.ToInt32(Baudrate.Text);
                serialPort1.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string getvalue;

        private void SendBtn_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (serialPort1.IsOpen)
                {       
                    serialPort1.WriteLine(InputField.Text + Environment.NewLine);
                    //InputField.Clear();
                    SerialMonitor.Text = serialPort1.ReadLine();
                    InputField.Clear();



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisconnectBtn_Click(object sender, EventArgs e)
        {
            ConnectBtn.Enabled = true;
            DisconnectBtn.Enabled = false;

            try
            {
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }


        private void ClearBtn_Click(object sender, EventArgs e)
        {
            SerialMonitor.Clear();
        }

        /// <summary>
        /// Marker!!!!
        /// </summary>
        private void DrawSignCurve()
        {
            PointPairList _pointPairList = new PointPairList();
            if (serialPort1.IsOpen)
            {
                for (int _angle = 0; _angle <= 1360; _angle = _angle + 10)
                {
                    double _x = _angle;
                    int TempYVal = 10;


                    TempYVal = serialPort1.ReadByte();



                    PointPair _pointPair = new PointPair(_x, serialPort1.ReadByte());
                    _pointPairList.Add(_pointPair);
                }
            }
         
            

            LineItem lineItem = GraphPane.AddCurve("Sine Curve", _pointPairList, Color.Red, SymbolType.None);
            zedGraphControl1.AxisChange();


        }
        /// <summary>
        /// M
        /// </summary>

        void runit()
        {
            while (true)
            {
                try
                {
                    if (serialPort1.IsOpen)
                    {
                        getvalue = serialPort1.ReadLine();
                        //chart1.Series["Series1"].Points.AddY(getvalue);

                        chart1.Invoke((MethodInvoker)(() => chart1.Series["Series1"].Points.AddXY(att, getvalue)));
                        //chart1.Series["Series1"].Points.AddXY(,Convert.ToInt32(getvalue));
                        //DateTime.Now.ToLongTimeString()

                    }
                }
                catch (Exception exc)
                {

                   
                }
            }
           
           

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            att++;

            if (CanCheck == true)
            {
                label1.Text = "true";
            }
            else
            {
                label1.Text = "false";
            }

            if (serialPort1.IsOpen && CanCheck == true)
            {
                _y = Convert.ToDouble(serialPort1.ReadLine());
                
               
            }
            else
            {
                //thinking wut to keep here //

                if (_y == null)
                {
                    NoOfTrys++;

                }
            }
            if (NoOfTrys > 5)
            {
                CanCheck = false;
            }

          //  CheckTime();
        }


        void CheckTime()
        {
         
            
            if (_y == null)
            {
                NoOfTrys++;

            }
            
           
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CanCheck = true;
        }
    }
    }
    

