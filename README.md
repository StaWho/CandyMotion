# CandyMotion
Motion detection in Silverlight

The idea is simple, we take two frames from the video feed (current and previous), convert them to grayscale and compare pixel-by-pixel. With this, whenever we find difference in pixel color, we will replace this pixel with fixed color one.

Moving to calculating difference between frames. For that, we will compare both frames pixel-by-pixel. If we detect any difference, we will replace such pixel with a color of our choice, if not, then we will simply copy existing pixel from one of our comparison frames. There's a problem we have to address however. What is a difference? Webcam feed does not provide best quality picture, also lighting conditions or reflective materials can cause the change in image brightness, which for us will eventually become foundation for detecting movement. We will address that by specifying a detection threshold. It is simply a required difference in brightness, and unless that difference is achieved, the pixel won't be replaced with solid color. This will let us tweak the algorithm to our lighting conditions. We also have to specify the replacement color.

Because we are replacing pixels, which we consider 'changed', with solid color ones, the more 'movement' we have (depending on out light conditions and threshold value) the more solid color pixels we have, right? Shall we just count them then? And if we have more than certain amount we can say there's definitely some 'movement' going on. EZ.
