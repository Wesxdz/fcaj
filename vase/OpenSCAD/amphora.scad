$fa = 0.1;
$fs = 0.5;
$fn = 60;

$to = 0.3;

vase_height = 26;
amphora_bottom_radius = 6;
amphora_lip_radius = 3.51;
inner_volume_dist_from_bottom = 2.0;
mid_radius = 8;
bot_radius = 11;
mid_vase_center = 15;
bot_vase_center = 12;
// The point at which the radius of the mid sphere becomes larger than the radius of bot

function radius_at_escape(r, p) = r * cos(p * PI/2);

function circle_intersection_h(h1, r1, h2, r2) = min(h1, h2) + abs(r1 - r2) + abs(h1 - h2);
    diff = h1-h2;
    h1Venn = h1 + r1;

mid_bot_intersection_height = circle_intersection(mid_vase_center, mid_radius, bot_vase_center, bot_radius);
bot_end_intersection_height = 10;

module volume_for_storage()
{
    
    translate([0,0,mid_vase_center]) scale([1,1,vase_height*.5/mid_radius]) sphere(r=mid_radius);
    translate([0,0,bot_vase_center]) sphere(r=bot_radius);
    
    translate([0,0,5]) cylinder(r=amphora_lip_radius, h=vase_height);

    // bottom flat face
    cylinder(h = mid_radius, r1 = amphora_bottom_radius, r2 = mid_radius, center = true/false);
}

module vase_body(s=1)
{
    scale([s,s,s]) volume_for_storage();
}

module flat_bottom()
{
    
}

module amphora_handle()
{
    scale([1,1,1])
    translate([0,5,20])
    rotate([0,90,0])
    rotate_extrude(convexity = 10)
    translate([5, 0, 0])
    circle(r = 0.8, $fn = 100);
}

module amphora_handles()
{
    rotate([0,0,20])
    amphora_handle();
    rotate([0,0,160])
    amphora_handle();
}

module amphora_lid()
{
    // airlock?
    translate([0,0,vase_height+1.5*3+1.5]) scale([1,1,0.5]) sphere(r=1.5);
    translate([0,0,vase_height+1.5*3]) cylinder(r=1.5, h=1.5);
    translate([0,0,vase_height+4.5]) cylinder(r=amphora_lip_radius + 1, h=0.5);
    translate([0,0,vase_height+3]) cylinder(r=amphora_lip_radius-0.5, h=1.5);
    // lip edge
    //translate([0,0,35]) linear_extrude() sphere(r=5.5, h=1.5);
}

// The following quotations are engraved on the Martin Luther King, Jr. Memorial:
// "Out of the mountain of despair, a stone of hope."
module amphora()
{
    difference()
    {   
        union()
        {
            difference()
            {
                union()
                {
                    amphora_handles();
                    difference()
                    {
                        vase_body(1.0);
                        remove_radius = 0.5;
                        rotate_extrude(convexity = 10)
                        translate([amphora_lip_radius + 0.5, vase_height + remove_radius * 1.5, 0])
                        circle(r = remove_radius, $fn = 100);
                        
                        remove_radius2 = 30.5;
                        rotate_extrude(convexity = 10)
                        translate([mid_radius, 20, 0])
                        circle(r = remove_radius2, $fn = 100);
                    }
                }
                union()
                {
                    translate([0,0,vase_height+inner_volume_dist_from_bottom]) cylinder(r=amphora_lip_radius-0.5,h=vase_height-inner_volume_dist_from_bottom);
                    translate([0.0, 0.0, 0.1]) vase_body(0.90);
                }
            }
            rotate_extrude(convexity = 10)
            translate([amphora_lip_radius, vase_height + 4.5, 0])
            circle(r = .5, $fn = 100);
            //translate([0,0,0]) amphora_lid();
        }
    }
}

module amphora_light_distribution()
{
    entries = 8;
}

amphora();