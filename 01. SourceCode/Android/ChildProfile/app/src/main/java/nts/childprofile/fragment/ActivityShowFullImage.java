package nts.childprofile.fragment;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;

import com.androidnetworking.widget.ANImageView;
import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;

import nts.childprofile.R;
import nts.childprofile.common.Utils;
import uk.co.senab.photoview.PhotoViewAttacher;

public class ActivityShowFullImage extends AppCompatActivity {
    private ImageView imageView = null;
    private Button buttonClose = null;
    private PhotoViewAttacher mAttacher;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_show_full_image);
//        fullScreen();
        buttonClose = (Button) findViewById(R.id.buttonClose);
        Intent intent = getIntent();
        String imageUrl = intent.getStringExtra("urlShowFullImage");
        imageView =  findViewById(R.id.imageView);

        if(imageUrl!=null){
            if (imageUrl.contains("http")) {
                displayImageOriginal(getApplication(), imageView, imageUrl);
            }else {
                Utils.AutoRotateImage(imageUrl);
                Utils.SetPicture(imageView, imageUrl);
            }
        }else {
//            imageView.setDefaultImageResId(R.drawable.ic_menu_gallery);
//            imageView.setErrorImageResId(R.drawable.ic_menu_slideshow);
        }

        mAttacher = new PhotoViewAttacher(imageView);
        mAttacher.setMaximumScale(200);
        buttonClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
    }

    public static void displayImageOriginal(Context ctx, ImageView img, String url) {
        try {
            Glide.with(ctx).load(url)
                    .crossFade()
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .into(img);
        } catch (Exception e) {
        }
    }
}
