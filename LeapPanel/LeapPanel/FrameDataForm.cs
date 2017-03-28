﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.VisualBasic;
using Leap;

namespace LeapPanel
{
    public partial class Form1 : Form, ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;

        //wifi vars
        private TcpClient tcpclnt;
        private string command;
        //private NetworkStream stream;
        private Byte[] data;

        //three fingers: index, middle, pinkle
        private Finger[] finger_object = new Finger[3];
        private Bone[,] bone_object = new Bone[3, 4];
        private bool[] finger_exist = new bool[3];

        private Finger thumb;
        private bool thumb_exist;
        private Bone thumb_meta;
        private Bone thumb_prox;
        private Bone thumb_inter;
    
        private Finger index;
  
        private Bone index_meta;
        private Bone index_prox;
        private Bone index_inter;
        private bool index_exist;
        private Finger middle;
      
        private Bone middle_meta;
        private Bone middle_prox;
        private Bone middle_inter;
        private bool middle_exist;

        int hand_position_x;
        int hand_position_y; 
        int hand_position_z;
        float roll;
        int roll_degrees;
        double thumb_PM_dot;
        double thumb_IP_dot;
        double thumb_pm_degree;
        double thumb_ip_degree;

        double index_PM_dot;
        double index_IP_dot;
        double index_pm_degree;
        double index_ip_degree;

        double middle_PM_dot;
        double middle_IP_dot;
        double middle_pm_degree;
        double middle_ip_degree;

        private bool start_track = true;
        private float time_track_1;
        //private float time_track_2;
        private double track_resolution = 250000;

        private int last_position_x = 0;
        private int last_position_y = 0;
        private int last_position_z = 0;

        public Form1()
        {
            InitializeComponent();

            ServerIPTextbox.Text = "192.168.1.244";
            PortNumber.Text = "2050";
            //wifi connection
            try
            {
                tcpclnt = new TcpClient();
                //Console.WriteLine("Connecting to the server[{0}]", "192.168.1.244");
                tcpclnt.Connect("192.168.1.244", 2050);
                //MessageBox.Show("Connected");
                //textBox5.Text = "Connected";
                //richTextBox.AppendText("Connected" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to server, reason : " +
                    ex.Message);
            }

            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
        }

        delegate void LeapEventDelegate(string EventName);
        public void LeapEventNotification(string EventName)
        {
            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                        break;
                    case "onConnect":
                        connectHandler();
                        break;
                    case "onFrame":
                        //greeting(this.controller.Frame());
                        fingerMotion(this.controller.Frame());
                        handMotion(this.controller.Frame());
                        fingerMotion_2(this.controller.Frame());
                        degree_calculation();
                        //degree_conversion();
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }

        public void degree_calculation()
        {
            
            if (thumb_meta != null && thumb_inter != null && thumb_prox != null)
            {
                Vector thumb_meta_norm = thumb_meta.Direction.Normalized;
                Vector thumb_prox_norm = thumb_prox.Direction.Normalized;
                Vector thumb_inter_norm = thumb_inter.Direction.Normalized;
                thumb_PM_dot = thumb_prox_norm.Dot(thumb_meta_norm);
                thumb_IP_dot = thumb_inter_norm.Dot(thumb_prox_norm);

                thumb_pm_degree = Math.Acos(thumb_PM_dot) * 180 /Math.PI;
                thumb_ip_degree = Math.Acos(thumb_IP_dot) * 180 / Math.PI;
                thumb_PM.Text = thumb_pm_degree.ToString();
                thumb_IP.Text = thumb_ip_degree.ToString();

                //Math.acos(dotProduct);
            }

            if (index_meta != null && index_inter != null & index_prox != null)
            {
                Vector index_meta_norm = index_meta.Direction.Normalized;
                Vector index_prox_norm = index_prox.Direction.Normalized;
                Vector index_inter_norm = index_inter.Direction.Normalized;
                index_PM_dot = index_prox_norm.Dot(index_meta_norm);
                index_IP_dot = index_inter_norm.Dot(index_prox_norm);

                index_pm_degree = Math.Acos(index_PM_dot) * 180 / Math.PI;
                index_ip_degree = Math.Acos(index_IP_dot) * 180 / Math.PI;
                index_PM.Text = index_pm_degree.ToString();
                index_IP.Text = index_ip_degree.ToString();
            }

            if (index_meta != null && index_inter != null & index_prox != null)
            {
                Vector middle_meta_norm = index_meta.Direction.Normalized;
                Vector middle_prox_norm = index_prox.Direction.Normalized;
                Vector middle_inter_norm = index_inter.Direction.Normalized;
                middle_PM_dot = middle_prox_norm.Dot(middle_meta_norm);
                middle_IP_dot = middle_inter_norm.Dot(middle_prox_norm);

                middle_pm_degree = Math.Acos(middle_PM_dot) * 180 / Math.PI;
                middle_ip_degree = Math.Acos(middle_IP_dot) * 180 / Math.PI;
                middle_PM.Text = middle_pm_degree.ToString();
                middle_IP.Text = middle_ip_degree.ToString();
            }
            // 
        }

        public void fingerMotion_2(Leap.Frame frame)
        {
            foreach (Finger finger in frame.Fingers)
            {
                //get finger type
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_THUMB:
                        thumb = finger;
                        thumb_exist = true;
                        thumb_text.Text = "detected";
                        bone_detect(finger);
                        break;

                    case Finger.FingerType.TYPE_INDEX:
                        index = finger;
                        index_exist = true;
                        index_text.Text = "detected";
                        bone_detect(finger);
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                        middle = finger;
                        middle_exist = true;
                        middle_text.Text = "detected";
                        bone_detect(finger);
                        break;
                }
            }
        }

        public void bone_detect(Finger finger)
        {
            foreach (Bone.BoneType boneType in (Bone.BoneType[])Enum.GetValues(typeof(Bone.BoneType)))
            {
                Bone bone = finger.Bone(boneType);

                switch (bone.Type)
                {
                    case Bone.BoneType.TYPE_METACARPAL:
                        bone_assignment_2(bone, finger);
                        break;

                    case Bone.BoneType.TYPE_PROXIMAL:
                        bone_assignment_2(bone, finger);
                        break;

                    case Bone.BoneType.TYPE_INTERMEDIATE:
                        bone_assignment_2(bone, finger);
                        break;

                    case Bone.BoneType.TYPE_DISTAL:
                        bone_assignment_2(bone, finger);
                        break;
                }
            }
        }

        public void bone_assignment_2(Bone bone, Finger finger)
        {

            if (bone.Type == Bone.BoneType.TYPE_METACARPAL)
            {
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_INDEX:
                        index_meta = bone;
                        indexmeta.Text = "detected";
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                        middle_meta = bone;
                        middlemeta.Text = "detected";
                        break;

                    case Finger.FingerType.TYPE_THUMB:
                        thumb_meta = bone;
                        thumbmeta.Text = "detected";
                        break;
                }
            }
            else if (bone.Type == Bone.BoneType.TYPE_PROXIMAL)
            {
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_INDEX:
                        index_prox = bone;
                        indexprox.Text = "detected";
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                       middle_prox = bone;
                       middleprox.Text = "detected";
                       break;

                    case Finger.FingerType.TYPE_THUMB:
                        thumb_prox = bone;
                        thumbprox.Text = "detected";
                        break;
                }
            }
            else if (bone.Type == Bone.BoneType.TYPE_INTERMEDIATE)
            {
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_INDEX:
                        index_inter = bone;
                        indexinter.Text = "detected";
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                        middle_inter = bone;
                        middleinter.Text = "detected";
                        break;

                    case Finger.FingerType.TYPE_THUMB:
                        thumb_inter = bone;
                        thumbinter.Text = "detected";
                        break;
                }
            }
        }

        public void connectHandler()
        {
            this.controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);

        }

        public void detectGesture(Leap.Frame frame)
        {
            GestureList gestures = frame.Gestures();

            for(int i = 0; i < gestures.Count(); i++)
            {
                Gesture gesture = gestures[i];

                switch(gesture.Type)
                {
                    case Gesture.GestureType.TYPE_INVALID:
                        break;

                    case Gesture.GestureType.TYPE_CIRCLE:
                        break;

                    case Gesture.GestureType.TYPE_SWIPE:
                        break;

                    case Gesture.GestureType.TYPE_KEY_TAP:
                        break;

                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        break;
                }
            }

 
        }

        public void handMotion(Leap.Frame frame)
        {
            Hand hand;
            Hand right_hand;

            //get hand roll degree
            if (frame.Hands.Count > 0)
            {
                hand = frame.Hands[0];
                right_hand = frame.Hands[1];

                if (hand.IsValid)
                {
                    Vector position = hand.PalmPosition;
                    Vector velocity = hand.PalmVelocity;
                    Vector direction = hand.Direction;
                    Vector normal = hand.PalmNormal;

                    roll = hand.PalmNormal.Roll;
                    roll_degrees = (int)ToDegrees(roll);

                    richTextBox3.Text = roll_degrees.ToString();

                    hand_position_x = (int)position.x;
                    hand_position_y = (int)position.y;
                    hand_position_z = (int)position.z;


                    if(last_position_x != (int)hand_position_x)
                    {   if(hand_position_x < 0)
                        {
                            xdirection.Text = "Left ";
                            x_displacement.Text = hand_position_x.ToString(); 
                        }
                        else
                        {
                            xdirection.Text = "Right";
                            x_displacement.Text = hand_position_x.ToString(); 
                        }
                        
                        last_position_x = (int)hand_position_x;
                    }

                    if (last_position_y != (int)hand_position_y)
                    {
                        hand_position_y = hand_position_y - 200;
                        if (hand_position_y < 0)
                        {
                            ydirection.Text = "Down";
                            y_displacement.Text = hand_position_y.ToString();
                        }
                        else
                        {
                            ydirection.Text = "Up";
                            y_displacement.Text = hand_position_y.ToString();
                        }

                        last_position_y = (int)hand_position_y;
                    }

                    if (last_position_z != (int)hand_position_z)
                    {
                        if (hand_position_z < 0)
                        {
                            zdirection.Text = "Forward";
                            z_displacement.Text = hand_position_z.ToString();
                        }
                        else
                        {
                            zdirection.Text = "Backward";
                            z_displacement.Text = hand_position_z.ToString();
                        }

                        last_position_z = (int)hand_position_z;
                    }
                }

                
            }
        }

        public void fingerMotion(Leap.Frame frame)
        {
            richTextBox.Text = frame.Timestamp.ToString();

            //char name = Console.ReadKey();

            //up_key.Text = name.ToString();

            if(start_track)
            {
                start_track = false;
                time_track_1 = frame.Timestamp;
            }
            
            if((frame.Timestamp - time_track_1) > track_resolution)
            {
 
                start_track = true;

                if (true)
                {
                    //finger 0(1)
                    if(thumb_exist == true)
                        command = "0" + "-" + "0" + "-" + ((int)thumb_pm_degree).ToString()
                                   + "-" + "1" + "-" + ((int)thumb_ip_degree).ToString() + "-" + "2"
                                   + "-" + ((int)thumb_ip_degree).ToString();
                    //add move command from keyboard
                    //add finger 1 and finger 2 data


                    System.Threading.Thread.Sleep(20);

                    if (tcpclnt.Connected)
                    {
                        NetworkStream stream = tcpclnt.GetStream();
                        if (command != null)
                        {
                            data = System.Text.Encoding.ASCII.GetBytes(command);
                            stream.Write(data, 0, data.Length);
                        }
                    }

                    //finger 1(2)
                    if(middle_exist == true)
                        command = "1" + "-" + "0" + "-" + ((int)middle_pm_degree).ToString()
                                   + "-" + "1" + "-" + ((int)middle_ip_degree).ToString() + "-" + "2"
                                   + "-" + ((int)middle_ip_degree).ToString();

                    if (tcpclnt.Connected)
                    {
                        NetworkStream stream = tcpclnt.GetStream();
                        if (command != null)
                        {
                            data = System.Text.Encoding.ASCII.GetBytes(command);
                            stream.Write(data, 0, data.Length);
                        }
                    }

                    System.Threading.Thread.Sleep(20);

                    //finger 2(3)
                    if(index_exist == true)
                        command = "2" + "-" + "0" + "-" + ((int)index_pm_degree).ToString()
                                   + "-" + "1" + "-" + ((int)index_ip_degree).ToString() + "-" + "2"
                                   + "-" + ((int)index_ip_degree).ToString();

                  
                    
                    if (tcpclnt.Connected)
                    {
                        NetworkStream stream = tcpclnt.GetStream();
                        if (command != null)
                        {
                            data = System.Text.Encoding.ASCII.GetBytes(command);
                            stream.Write(data, 0, data.Length);
                        }
                    }

                    System.Threading.Thread.Sleep(20);
                }

                //finger_x_plane();


                richTextBox2.Text = frame.Timestamp.ToString();
                
            }

            thumb_exist = false;
            index_exist = false;
            middle_exist = false;
         }

        public float ToDegrees(float Radian)
        {
            float degrees;
            degrees = Radian * 180 / (float)Math.PI;
            return degrees;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox_d1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void up_key_KeyDown(object sender, KeyEventArgs e)
        {
            //string G_str_Mode = "";
            string G_str_text = e.KeyCode + ":" + e.Modifiers + ":" + e.KeyData + ":" + "(" + e.KeyValue + ")";
           
            this.Text = G_str_text;  
             
        }
    }

    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }

    public class LeapEventListener : Listener
    {
        ILeapEventDelegate eventDelegate;

        public LeapEventListener(ILeapEventDelegate delegateObject)
        {
            this.eventDelegate = delegateObject;
        }
        public override void OnInit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onInit");
        }
        public override void OnConnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onConnect");
        }
        public override void OnDeviceChange(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onDevice");
        }
        public override void OnFrame(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onFrame");
        }
        public override void OnExit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onExit");
        }
        public override void OnDisconnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onDisconnect");
        }
    }
}
