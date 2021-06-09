package nts.swipesafe.adapter;

import android.content.Context;
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
public class ImageAdapter extends RecyclerView.Adapter<ImageAdapter.MyViewHolder> {

    private ArrayList<GalleryModel> mImagesList, mImageChooseList;
    private Context mContext;
    private ImageAdapter.OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, boolean isCheck, GalleryModel galleryModel);
    }

    public void SetOnItemClickListener(final ImageAdapter.OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public ImageAdapter(Context context, ArrayList<GalleryModel> imageList, ArrayList<GalleryModel> imageChooseList) {
        mContext = context;
        mImagesList = new ArrayList<GalleryModel>();
        this.mImagesList = imageList;
        mImageChooseList = imageChooseList;
    }


    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View itemView = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_multiphoto, parent, false);

        return new MyViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(final MyViewHolder holder, final int position) {

        final GalleryModel galleryModel = mImagesList.get(position);
        //holder.imageView.setImageBitmap(image);
        try {

            if (galleryModel.Type == 1) {
                Picasso.with(mContext)
                        .load("file://" + galleryModel.FileUrl)
                        .resize(500, 500)
                        .placeholder(R.drawable.ic_image)
                        .error(R.drawable.ic_image)
                        .centerCrop().into(holder.imgThumbnail);
                holder.imgVideo.setVisibility(View.GONE);
            } else if (galleryModel.Type == 3 && galleryModel.ThumbnailVideo != null) {
                holder.imgThumbnail.setImageBitmap(galleryModel.ThumbnailVideo);
                holder.imgVideo.setVisibility(View.VISIBLE);
            }

            if (mImageChooseList != null && mImageChooseList.contains(galleryModel)) {
                holder.checkFile.setChecked(true);
            } else {
                holder.checkFile.setChecked(false);
            }

        } catch (Exception ex) {
        }

        holder.lyFile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    CheckBox checkBox = (CheckBox) v.findViewById(R.id.cbCheck);
                    boolean isCheck = !holder.checkFile.isChecked();
                    holder.checkFile.setChecked(!holder.checkFile.isChecked());
                    mOnItemClickListener.onItemClick(v, position, isCheck, galleryModel);
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return mImagesList.size();
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {

        public CheckBox checkFile;
        public ImageView imgThumbnail, imgVideo;
        public RelativeLayout lyFile;

        public MyViewHolder(View view) {
            super(view);

            checkFile = (CheckBox) view.findViewById(R.id.checkFile);
            imgThumbnail = (ImageView) view.findViewById(R.id.imgThumbnail);
            imgVideo = (ImageView) view.findViewById(R.id.imgVideo);
            lyFile = (RelativeLayout) view.findViewById(R.id.lyFile);
        }
    }

}
