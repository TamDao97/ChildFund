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
import nts.swipesafe.model.SkillModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class SkillAdapter extends RecyclerView.Adapter<SkillAdapter.ViewHolder> {

    private final int mBackground;
    private List<SkillModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, SkillModel skillModel);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout lySkill;
        public TextView txtTitle, txtDateTime, txtSubTitle;
        public ImageView imageThumbnail;

        public ViewHolder(View v) {
            super(v);
            lySkill = (LinearLayout) v.findViewById(R.id.lySkill);
            txtTitle = (TextView) v.findViewById(R.id.txtTitle);
            txtDateTime = (TextView) v.findViewById(R.id.txtDateTime);
            txtSubTitle = (TextView) v.findViewById(R.id.txtSubTitle);
            imageThumbnail = (ImageView) v.findViewById(R.id.imageThumbnail);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public SkillAdapter(Context ctx, List<SkillModel> items) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public SkillAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_skill, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final SkillModel skillModel = filtered_items.get(position);
        holder.txtTitle.setText(skillModel.Title);
        holder.txtSubTitle.setText(skillModel.SubTitle);
        holder.txtDateTime.setText(skillModel.DateTime);
        int imageId = ctx.getResources().getIdentifier(skillModel.ImageThumbnail, "drawable", ctx.getPackageName());
        Resources resources = ctx.getResources();
        holder.imageThumbnail.setImageDrawable(resources.getDrawable(imageId));
        // view detail message conversation
        holder.lySkill.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(v, position, skillModel);
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
