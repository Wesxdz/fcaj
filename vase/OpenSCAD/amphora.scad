$fa = 1;
$fs = 0.5;
$fn = 40;

$to = 0.3;

vase_height = 26;
amophora_lip_radius = 5;
inner_volume_dist_from_bottom = 2.0;

module volume_for_storage()
{
    translate([0,0,10*1.5]) scale([1,1,1.5]) sphere(r=10);
    
    translate([0,0,10]) sphere(r=10);
    
    translate([0,0,5]) cylinder(r=amophora_lip_radius, h=vase_height);
    
    cylinder(h = 10, r1 = amophora_lip_radius, r2 = 10, center = true/false);
}

module vase_body(s=1)
{
    scale([s,s,s]) volume_for_storage();
}

module flat_bottom()
{
    
}

module amphora_lid()
{
    // airlock?
    translate([0,0,vase_height+1.5*3]) cylinder(r=1.5, h=3.5);
    translate([0,0,vase_height+3]) cylinder(r=amophora_lip_radius, h=1.5);
    // lip edge
    //translate([0,0,35]) linear_extrude() sphere(r=5.5, h=1.5);
}

// The following quotations are engraved on the Martin Luther King, Jr. Memorial:
// "Out of the mountain of despair, a stone of hope."
module amphora()
{
    union()
    {
        difference()
        {
            vase_body(1.0);
            translate([0,0,vase_height+inner_volume_dist_from_bottom]) cylinder(r=amophora_lip_radius-0.5,h=vase_height-inner_volume_dist_from_bottom);
            translate([0.0, 0.0, 0.1]) vase_body(0.90);
        }
        translate([0,0,0]) amphora_lid();
    }
}

module amphora_light_distribution()
{
    entries = 8;
}

amphora();