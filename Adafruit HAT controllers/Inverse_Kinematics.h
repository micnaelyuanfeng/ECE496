#ifndef INVERSE_KINEMATICS
#define INVERSE_KINEMATICS
#define d1  15   //ground to q1
#define d6  20 //gripper to wrist
#define a_2 10    //q1 to q2
#define a_3 10  //q2 to wrist
double angles[7];
long double t[3][3]= {{0,1,0},    //the target rotation matrix R
                      {0,0,1},
                      {1,0,0}};

void inverseKinematics(double x,double y,double z,double t[3][3],double angles[7]);

#endif