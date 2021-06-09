package nts.swipesafe.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.VideoModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class YoutobeVideoAdapter extends RecyclerView.Adapter<YoutobeVideoAdapter.ViewHolder> {

    private final int mBackground;
    private List<VideoModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, VideoModel videoModel);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout lyVideo;
        public TextView txtTitle, txtDateTime, txtSubTitle;
        public ImageView imageThumbnail;

        public ViewHolder(View v) {
            super(v);
            lyVideo = (LinearLayout) v.findViewById(R.id.lyVideo);
            txtTitle = (TextView) v.findViewById(R.id.txtTitle);
            txtDateTime = (TextView) v.findViewById(R.id.txtDateTime);
            imageThumbnail = (ImageView) v.findViewById(R.id.imageThumbnail);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public YoutobeVideoAdapter(Context ctx, List<VideoModel> items) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public YoutobeVideoAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_video, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final VideoModel videoModel = filtered_items.get(position);
        holder.txtTitle.setText(videoModel.Title);
        holder.txtDateTime.setText(DateUtils.ConvertYMDHHmmServerToDMYHHmm(videoModel.DateTime));
        Picasso.with(ctx)
                .load(videoModel.ImageThumbnail)
                .resize(120, 90)
                .placeholder(R.drawable.ic_image)
                .error(R.drawable.ic_image)
                .centerCrop().into(holder.imageThumbnail);
        // view detail message conversation
        holder.lyVideo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(v, position, videoModel);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return filtered_items.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
