package nts.swipesafe.adapter;

import android.content.Context;
import android.content.res.Resources;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.model.ChildAbuseModel;
import nts.swipesafe.model.WebsiteModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class LinkWebsiteAdapter extends RecyclerView.Adapter<LinkWebsiteAdapter.ViewHolder> {

    private final int mBackground;
    private List<WebsiteModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, WebsiteModel websiteModel);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout lyLinkWebsite;
        public TextView txtTitle;
        public ImageView imgIcon;

        public ViewHolder(View v) {
            super(v);
            lyLinkWebsite = (LinearLayout) v.findViewById(R.id.lyLinkWebsite);
            txtTitle = (TextView) v.findViewById(R.id.txtTitle);
            imgIcon = (ImageView) v.findViewById(R.id.imgIcon);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public LinkWebsiteAdapter(Context ctx, List<WebsiteModel> items) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public LinkWebsiteAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_webseite, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final WebsiteModel websiteModel = filtered_items.get(position);
        int imageId = ctx.getResources().getIdentifier(websiteModel.Icon, "drawable", ctx.getPackageName());
        Resources resources = ctx.getResources();
        holder.imgIcon.setImageDrawable(resources.getDrawable(imageId));
        holder.txtTitle.setText(websiteModel.Name);
        // view detail message conversation
        holder.lyLinkWebsite.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(v, position, websiteModel);
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
