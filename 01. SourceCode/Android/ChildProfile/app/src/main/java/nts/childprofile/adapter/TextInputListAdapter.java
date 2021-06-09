package nts.childprofile.adapter;

import android.content.Context;
import android.support.v7.widget.AppCompatEditText;
import android.support.v7.widget.RecyclerView;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import nts.childprofile.R;
import nts.childprofile.model.ObjectInputModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class TextInputListAdapter extends RecyclerView.Adapter<TextInputListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ObjectInputModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onTextChange(int position, String obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public EditText txtName;
        public  TextView lblName;

        public ViewHolder(View v) {
            super(v);
            lblName = v.findViewById(R.id.lblName);
            txtName =  v.findViewById(R.id.txtName);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public TextInputListAdapter(Context ctx, List<ObjectInputModel> items) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public TextInputListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_text_input, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final ObjectInputModel checkBoxModel = filtered_items.get(position);
        holder.lblName.setText(checkBoxModel.Name);
        holder.txtName.setText(checkBoxModel.Value);
        holder.txtName.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onTextChange(position, s.toString());
                }
            }

            @Override
            public void afterTextChanged(Editable s) {

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
