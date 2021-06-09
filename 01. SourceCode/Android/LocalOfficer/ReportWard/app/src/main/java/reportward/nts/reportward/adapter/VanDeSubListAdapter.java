package reportward.nts.reportward.adapter;

import android.content.Context;
import android.content.DialogInterface;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
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
public class VanDeSubListAdapter extends RecyclerView.Adapter<VanDeSubListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ItemObjectInSubModel> filtered_items = new ArrayList<>();
    private List<ComboboxResult> mListChoose;
    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemCheckClick(View view, int position, ItemObjectInSubModel itemObjectInSubModel, boolean isCheck);

        void onChooseOkClick(List<ComboboxResult> listChoose, List<ComboboxResult> listUnChoose);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtName;
        public LinearLayout lyObjectType;
        private ImageView imgShowChoose;
        private CheckBox cbCheck;

        public ViewHolder(View v) {
            super(v);
            txtName = v.findViewById(R.id.txtName);
            lyObjectType = v.findViewById(R.id.lyObjectType);
            cbCheck = v.findViewById(R.id.cbCheck);
            imgShowChoose = v.findViewById(R.id.imgShowChoose);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public VanDeSubListAdapter(Context ctx, List<ItemObjectInSubModel> items, List<ComboboxResult> listChoose) {
        this.ctx = ctx;
        filtered_items = items;
        mListChoose = listChoose;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public VanDeSubListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_list_sub_choose, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(final ViewHolder holder, final int position) {
        final ItemObjectInSubModel itemObjectInSubModel = filtered_items.get(position);
        if (itemObjectInSubModel.listItem != null && itemObjectInSubModel.listItem.size() > 0) {
            holder.cbCheck.setVisibility(View.GONE);
        } else {
            holder.imgShowChoose.setVisibility(View.GONE);
            holder.cbCheck.setChecked(false);
            if (mListChoose != null && mListChoose.size() > 0) {
                for (ComboboxResult itemCheck : mListChoose) {
                    if (itemCheck.id.equals(itemObjectInSubModel.id)) {
                        holder.cbCheck.setChecked(true);
                        break;
                    }
                }
            }
        }
        holder.txtName.setText(itemObjectInSubModel.text);
        // view detail message conversation
        holder.lyObjectType.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mOnItemClickListener != null) {
                    if (itemObjectInSubModel.listItem != null && itemObjectInSubModel.listItem.size() > 0) {
                        holder.cbCheck.setVisibility(View.GONE);
                        String[] arrayName = new String[itemObjectInSubModel.listItem.size()];
                        boolean[] arrayCheck = new boolean[itemObjectInSubModel.listItem.size()];
                        String[] arrayId = new String[itemObjectInSubModel.listItem.size()];
                        int index = 0;
                        for (ItemObjectInSubModel item : itemObjectInSubModel.listItem) {
                            arrayName[index] = item.text;
                            arrayId[index] = item.id;
                            arrayCheck[index] = false;
                            if (mListChoose != null && mListChoose.size() > 0) {
                                for (ComboboxResult itemCheck : mListChoose) {
                                    if (itemCheck.id.equals(item.id)) {
                                        arrayCheck[index] = true;
                                        break;
                                    }
                                }
                            }
                            index++;
                        }
                        showMultiChooseDialog(arrayName, arrayCheck, arrayId, "Chọn các vấn đề", null);
                    } else {
                        holder.cbCheck.setChecked(!holder.cbCheck.isChecked());
                        if (mOnItemClickListener != null) {
                            mOnItemClickListener.onItemCheckClick(v, position, itemObjectInSubModel, holder.cbCheck.isChecked());
                        }
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

    /***
     * Show Dialog mutile choose
     * @param arrayName
     * @param title
     * @param funtion
     */
    private void showMultiChooseDialog(final String[] arrayName, final boolean[] arrayCheck, final String[] arrayId, final String title, final Runnable funtion) {
        final android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(ctx);
        builder.setTitle(title);
        builder.setCancelable(true);
        builder.setMultiChoiceItems(arrayName, arrayCheck, new DialogInterface.OnMultiChoiceClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which, boolean isChecked) {
                arrayCheck[which] = isChecked;
            }
        });

        // Set the positive/yes button click listener
        builder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                if (mOnItemClickListener != null) {
                    List<ComboboxResult> listCheck = new ArrayList<>();
                    List<ComboboxResult> listUnCheck = new ArrayList<>();
                    ComboboxResult comboboxResult;
                    for (int i = 0; i < arrayCheck.length; i++) {
                        boolean checked = arrayCheck[i];
                        comboboxResult = new ComboboxResult();
                        comboboxResult.text = arrayName[i];
                        comboboxResult.id = arrayId[i];
                        if (checked) {
                            listCheck.add(comboboxResult);
                        } else {
                            listUnCheck.add(comboboxResult);
                        }
                    }
                    mOnItemClickListener.onChooseOkClick(listCheck, listUnCheck);
                }
            }
        });

        // Set the neutral/cancel button click listener
        builder.setNeutralButton("Cancel", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                // Do something when click the neutral button
            }
        });

        builder.show();
    }
}
