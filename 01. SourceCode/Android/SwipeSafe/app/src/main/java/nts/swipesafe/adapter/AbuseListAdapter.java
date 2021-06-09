package nts.swipesafe.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.model.ChildAbuseModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class AbuseListAdapter extends RecyclerView.Adapter<AbuseListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ChildAbuseModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;
    private boolean isCheck;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, boolean isCheck);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtName;
        public CheckBox cbCheck;
        public LinearLayout lyAbuse;

        public ViewHolder(View v) {
            super(v);
            txtName = (TextView) v.findViewById(R.id.txtName);
            if (isCheck) {
                cbCheck = (CheckBox) v.findViewById(R.id.cbCheck);
            }
            lyAbuse = (LinearLayout) v.findViewById(R.id.lyAbuse);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public AbuseListAdapter(Context ctx, List<ChildAbuseModel> items, boolean isCheck) {
        this.ctx = ctx;
        this.isCheck = isCheck;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public AbuseListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(isCheck ? R.layout.item_abuse_check : R.layout.item_abuse_detail, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final ChildAbuseModel checkBoxModel = filtered_items.get(position);
        holder.txtName.setText(checkBoxModel.AbuseName);
        if (isCheck) {
            holder.cbCheck.setChecked(checkBoxModel.IsCheck);
            // view detail message conversation
            holder.lyAbuse.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    if (mOnItemClickListener != null) {
                        CheckBox checkBox = (CheckBox) v.findViewById(R.id.cbCheck);
                        checkBox.setChecked(!checkBox.isChecked());
                        mOnItemClickListener.onItemClick(v, position, checkBox.isChecked());
                    }
                }
            });
        }
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
