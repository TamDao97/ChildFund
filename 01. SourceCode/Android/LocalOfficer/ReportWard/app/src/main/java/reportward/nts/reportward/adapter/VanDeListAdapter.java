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
import reportward.nts.reportward.model.ComboboxResult;
import reportward.nts.reportward.model.ItemObjectInSubModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class VanDeListAdapter extends RecyclerView.Adapter<VanDeListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ItemObjectInSubModel> filtered_items = new ArrayList<>();
    private List<ComboboxResult> mListChoose = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onCheckClick(boolean isCheck, ItemObjectInSubModel childObjectTypeModel);

        void onChooseClick(List<ComboboxResult> listChoose, List<ComboboxResult> listUnChoose);
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
            imgExpand.setImageResource(R.drawable.ic_keyboard_arrow_up);
            lyObjectType = v.findViewById(R.id.lyObjectType);
            lyObjectChildSub = v.findViewById(R.id.lyObjectChildSub);
            rvObjectChildSub = v.findViewById(R.id.rvObjectChildSub);
            rvObjectChildSub.setLayoutManager(new GridLayoutManager(ctx, 1));
            rvObjectChildSub.setHasFixedSize(true);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public VanDeListAdapter(Context ctx, List<ItemObjectInSubModel> items, List<ComboboxResult> listChoose) {
        this.ctx = ctx;
        filtered_items = items;
        mListChoose = listChoose;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public VanDeListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
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
        final ItemObjectInSubModel itemObjectInSubModel = filtered_items.get(position);
        holder.txtName.setText(itemObjectInSubModel.text);
        VanDeSubListAdapter vanDeSubListAdapter = new VanDeSubListAdapter(ctx, itemObjectInSubModel.listItem, mListChoose);
        holder.rvObjectChildSub.setAdapter(vanDeSubListAdapter);
        vanDeSubListAdapter.SetOnItemClickListener(new VanDeSubListAdapter.OnItemClickListener() {
            @Override
            public void onItemCheckClick(View view, int position, ItemObjectInSubModel itemObjectInSubModel, boolean isCheck) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onCheckClick(isCheck,itemObjectInSubModel);
                }
            }

            @Override
            public void onChooseOkClick(List<ComboboxResult> listChoose, List<ComboboxResult> listUnChoose) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onChooseClick(listChoose, listUnChoose);
                }
            }
        });
        // view detail message conversation
        holder.lyObjectType.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    itemObjectInSubModel.isCheck = !itemObjectInSubModel.isCheck;
                    if (itemObjectInSubModel.isCheck) {
                        holder.lyObjectChildSub.setVisibility(View.VISIBLE);
                        holder.imgExpand.setImageResource(R.drawable.ic_keyboard_arrow_down);
                    } else {
                        holder.lyObjectChildSub.setVisibility(View.GONE);
                        holder.imgExpand.setImageResource(R.drawable.ic_keyboard_arrow_up);
                    }
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
