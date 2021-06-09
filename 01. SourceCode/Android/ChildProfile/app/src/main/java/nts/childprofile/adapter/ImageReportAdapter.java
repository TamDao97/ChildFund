package nts.childprofile.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.RelativeLayout;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;

import nts.childprofile.R;
import nts.childprofile.model.GalleryModel;

/**
 * @author Paresh Mayani (@pareshmayani)
 */
public class ImageReportAdapter extends RecyclerView.Adapter<ImageReportAdapter.MyViewHolder> {

    private ArrayList<GalleryModel> mImagesList;
    private Context mContext;
    private ImageReportAdapter.OnItemClickListener mOnItemClickListener;
    private boolean mIsViewDelete;

    public interface OnItemClickListener {
        void onRemoveClick(View view, int position, GalleryModel imageUrl);

        void showFullImage(View view, int position, GalleryModel imageUrl);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public ImageReportAdapter(Context context, ArrayList<GalleryModel> imageList, boolean isViewDelete) {
        mContext = context;
        mImagesList = new ArrayList<GalleryModel>();
        this.mImagesList = imageList;
        mIsViewDelete = isViewDelete;
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View itemView = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_photo_report, parent, false);

        return new MyViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(final MyViewHolder holder, final int position) {

        final GalleryModel galleryModel = mImagesList.get(position);
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

        if (mIsViewDelete) {
            holder.imRemove.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    if (mOnItemClickListener != null) {
                        mOnItemClickListener.onRemoveClick(view, position, galleryModel);
                    }
                }
            });
        } else {
            holder.imRemove.setVisibility(View.GONE);
        }

        holder.imgThumbnail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mOnItemClickListener.showFullImage(v, position, galleryModel);
            }
        });
    }

    @Override
    public int getItemCount() {
        return mImagesList.size();
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        public ImageView imgThumbnail, imgVideo, imRemove;
        public RelativeLayout lyFile;

        public MyViewHolder(View view) {
            super(view);

            imRemove = (ImageView) view.findViewById(R.id.imRemove);
            imgThumbnail = (ImageView) view.findViewById(R.id.imgThumbnail);
            imgVideo = (ImageView) view.findViewById(R.id.imgVideo);
            lyFile = (RelativeLayout) view.findViewById(R.id.lyFile);
        }
    }

}
