void inverseKinematics(double x,double y,double z,double t[3][3],double angles[7]) /*results are stored in angles array */
{
    /* the method here follows the calculations from the book:
    Modelling and Control of Robot Manipulators, Lorenzo Sciavicco and Bruno Siciliano chapter 2 (specifically 2.12.2)*/
    double xc,yc,zc;
    double ay,ax,az,sz,nz;
    double r11,r12,r13,r21,r22,r23,r31,r32,r33;
    double q1,q2,q3;
    double D,k1,k2;
    /* compute the wrist position */
    xc = x - d6*t[0][2];
    yc = y - d6*t[1][2];
    zc = z - d6*t[2][2];
    /* solve inverse kinematics for the first 3 angles found in: */
    /* http://www.hessmer.org/uploads/RobotArm/Inverse%2520Kinematics%2520for%2520Robot%2520Arm.pdf */
    D = ( pow(xc,2) + pow(yc,2) + pow((zc-d1),2) - pow(a_2,2) - pow(a_3,2) )/(2*a_2*a_3);
	angles[1] = atan2l(yc,xc);
    angles[3] = atan2l( -sqrt( abs(1 - pow(D,2)) ),D );
    k1 = a_2+a_3*cosl(angles[3]);
    k2 = a_3*sinl(angles[3]);

    angles[2] = atan2l( (zc-d1), sqrt(pow(xc,2) + pow(yc,2)) ) - atan2l(k2,k1) ;
    /* the DH frame is rotated 90 degrees compared to the calculated value see 2.9.7 from the book*/
    angles[3] = angles[3] + M_PI/2;

    /* for my own sanity */
    q1=angles[1]; q2=angles[2]; q3=angles[3];
    r11=t[0][0];r12=t[0][1];r13=t[0][2]; r21=t[1][0];r22=t[1][1];r23=t[1][2]; r31=t[2][0];r32=t[2][1];r33=t[2][2];

    /* solve inverse kinematics for the final 3 angles (2.12.5 from the book) */
    ax = r13*cosl(q1)*cosl(q2 + q3) + r23*cosl(q2 + q3)*sinl(q1) + r33*sinl(q2 + q3);
    ay = -r23*cosl(q1) + r13*sinl(q1);
    az = -r33*cosl(q2 + q3) + r13*cosl(q1)*sinl(q2 + q3) + r23*sinl(q1)*sinl(q2 + q3);
    sz = -r32*cosl(q2 + q3) + r12*cosl(q1)*sinl(q2 + q3) + r22*sinl(q1)*sinl(q2 + q3);
    nz = -r31*cosl(q2 + q3) + r11*cosl(q1)*sinl(q2 + q3) + r21*sinl(q1)*sinl(q2 + q3);

    /* getting the angles from the matrix only works if ax != 0 and ay != 0 */
    if(ax == 0)
        ax += 0.0000001;
    if(ay == 0)
      ay += 0.0000001;

    angles[4] = atan2l(-ay,-ax);
    angles[5] = atan2l(-sqrt(ax*ax+ay*ay),az);
    angles[6] = atan2l(-sz,nz);

    /* all that follows now is fixing the angles because some of the servo orientations */
    /* do no align with the DH frames and servo's can only move 180 degrees*/
    if (angles[4] >  M_PI/2){
    angles[4] = angles[4] -  M_PI;
    angles[5] = -angles[5];
    angles[6] =  M_PI + angles[6];
    }
    if (angles[4] < - M_PI/2){
    angles[4] = angles[4] +  M_PI;
    angles[5] = -angles[5];
    angles[6] =  M_PI + angles[6];
    }
    angles[1] = angles[1];
    angles[2] = angles[2];
    angles[3] = -(angles[3] -  M_PI/2);
    angles[4] = (angles[4] +  M_PI/2);
    angles[5] = ( M_PI/2) - angles[5];
    angles[6] = fmod( (( M_PI/2) + angles[6] ), 2.0* M_PI) ;
}
