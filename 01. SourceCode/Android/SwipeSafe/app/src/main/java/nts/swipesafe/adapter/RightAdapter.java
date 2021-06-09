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

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.model.RightModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class RightAdapter extends RecyclerView.Adapter<RightAdapter.ViewHolder> {

    private final int mBackground;
    private List<RightModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, RightModel rightModel);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout lyRight;
        public TextView txtTitle, txtDateTime;
        public ImageView imDownload;

        public ViewHolder(View v) {
            super(v);
            lyRight = (LinearLayout) v.findViewById(R.id.lyRight);
            txtTitle = (TextView) v.findViewById(R.id.txtTitle);
            txtDateTime = (TextView) v.findViewById(R.id.txtDateTime);
            imDownload = (ImageView) v.findViewById(R.id.imDownload);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public RightAdapter(Context ctx, List<RightModel> items) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public RightAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_right, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final RightModel rightModel = filtered_items.get(position);
        holder.txtTitle.setText(rightModel.Title);
        holder.txtDateTime.setText(rightModel.DateTime);
        // view detail message conversation
        holder.imDownload.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(v, position, rightModel);
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
