package nts.swipesafe.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.ImageView;
import android.widget.RelativeLayout;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;

import nts.swipesafe.R;
import nts.swipesafe.model.GalleryModel;

/**
 * @author Paresh Mayani (@pareshmayani)
 */
public class ImageInternetAdapter extends RecyclerView.Adapter<ImageInternetAdapter.MyViewHolder> {

    private String[] mImagesList;
    private Context mContext;

    public ImageInternetAdapter(Context context, String[] imageList) {
        mContext = context;
        this.mImagesList = imageList;
    }


    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View itemView = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_image, parent, false);

        return new MyViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(final MyViewHolder holder, final int position) {

        final String imageName = mImagesList[position];
        //holder.imageView.setImageBitmap(image);
        try {

            int imageId = mContext.getResources().getIdentifier(imageName, "drawable", mContext.getPackageName());
            Resources resources = mContext.getResources();
            holder.imgThumbnail.setImageDrawable(resources.getDrawable(imageId));

        } catch (Exception ex) {
        }
    }

    @Override
    public int getItemCount() {
        return mImagesList.length;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {

        public ImageView imgThumbnail;

        public MyViewHolder(View view) {
            super(view);

            imgThumbnail = (ImageView) view.findViewById(R.id.imgThumbnail);
        }
    }

}
