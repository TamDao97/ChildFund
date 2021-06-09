package nts.swipesafe.model;

import android.graphics.Bitmap;

public class GalleryModel {
    //Uri file
    public String FileUrl;

    ///Anh Thumbnail video
    public Bitmap ThumbnailVideo;

    ///1:image, 3: video
    public int Type;

    //Kích thức file byte
    public long Size;
}
