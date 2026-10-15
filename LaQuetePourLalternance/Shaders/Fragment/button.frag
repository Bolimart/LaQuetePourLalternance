#version 460 core 

// The fragment shader only requires one output variable and that is a vector of size 4
out vec4 FragColor; 

in vec2 uvCoord;
uniform bool isHovered;
uniform bool isClicked;
uniform vec4 hoveredColor;
uniform vec4 clickedColor;
uniform sampler2D texture0;

void main()
{
    if (isHovered){
        FragColor = mix(texture(texture0, uvCoord), hoveredColor, 0.2 * texture(texture0, uvCoord).a);
    }
    else if (isClicked){
        FragColor = mix(texture(texture0, uvCoord), clickedColor, 0.2 * texture(texture0, uvCoord).a);
    }
    else{
        FragColor = texture(texture0, uvCoord);
    }
}