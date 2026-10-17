#version 460 core 

// The fragment shader only requires one output variable and that is a vector of size 4
    out vec4 FragColor; 

in vec2 uvCoord;

uniform vec3 color;
uniform sampler2D texture0;

void main()
{
    FragColor = vec4(color.xyz, texture(texture0, uvCoord).a);
}