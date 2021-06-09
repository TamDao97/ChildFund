package reportward.nts.reportward.adapter;

import android.content.Context;
import android.support.v7.widget.GridLayoutManager;
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

import reportward.nts.reportward.R;
import reportward.nts.reportward.model.ItemObjectInSubModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class ChildObjectTypeListAdapter extends RecyclerView.Adapter<ChildObjectTypeListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ItemObjectInSubModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, ItemObjectInSubModel childObjectTypeModel);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtName;
        public ImageView imgExpand;
        public LinearLayout lyObjectType, lyObjectChildSub;
        public RecyclerView rvObjectChildSub;

        public ViewHolder(View v) {
            super(v);
            txtName = v.findViewById(R.id.txtName);
            imgExpand = v.findViewById(R.id.imgExpand);
            lyObjectType = v.findViewById(R.id.lyObjectType);
            lyObjectChildSub = v.findViewById(R.id.lyObjectChildSub);
            rvObjectChildSub = v.findViewById(R.id.rvObjectChildSub);
            rvObjectChildSub.setLayoutManager(new GridLayoutManager(ctx, 1));
            rvObjectChildSub.setHasFixedSize(true);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public ChildObjectTypeListAdapter(Context ctx, List<ItemObjectInSubModel> items) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public ChildObjectTypeListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_child_object_type_group, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(final ViewHolder holder, final int position) {
        final ItemObjectInSubModel childObjectTypeModel = filtered_items.get(position);
        holder.txtName.setText(childObjectTypeModel.text);
        //holder.imgExpand.setChecked(childObjectTypeModel.isCheck);
        //VanDeSubListAdapter childListAdapter = new VanDeSubListAdapter(ctx, childObjectTypeModel.listItem);
        //holder.rvObjectChildSub.setAdapter(childListAdapter);
        // view detail message conversation
        holder.lyObjectType.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(v,position,childObjectTypeModel);
//                    childObjectTypeModel.isCheck = !childObjectTypeModel.isCheck;
//                    if(childObjectTypeModel.isCheck) {
//                        holder.lyObjectChildSub.setVisibility(View.VISIBLE);
//                        holder.imgExpand.setImageResource(R.drawable.ic_keyboard_arrow_down);
//                    }else {
//                        holder.lyObjectChildSub.setVisibility(View.GONE);
//                        holder.imgExpand.setImageResource(R.drawable.ic_keyboard_arrow_up);
//                    }
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
