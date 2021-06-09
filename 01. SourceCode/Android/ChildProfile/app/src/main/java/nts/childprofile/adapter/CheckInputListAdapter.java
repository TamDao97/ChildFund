package nts.childprofile.adapter;

import android.content.Context;
import android.graphics.Color;
import android.support.design.widget.TextInputLayout;
import android.support.v4.content.ContextCompat;
import android.support.v7.widget.RecyclerView;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import nts.childprofile.R;
import nts.childprofile.model.ObjectInputModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class CheckInputListAdapter extends RecyclerView.Adapter<CheckInputListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ObjectInputModel> filtered_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;
    private boolean mIsHorizontal;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, ObjectInputModel obj);

        void onTextChange(int position, String obj);

        void onRadioCheck(int position, RadioButton obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtName, txtQuestion;
        public CheckBox cbCheck;
        public EditText txtOther;
        public TextInputLayout tilOther;
        public LinearLayout lyQuestion;
        public RadioButton radioYes, radioNo;

        public ViewHolder(View v) {
            super(v);
            txtName = v.findViewById(R.id.txtName);
            cbCheck = v.findViewById(R.id.cbCheck);
            txtOther = v.findViewById(R.id.txtOther);
            tilOther = v.findViewById(R.id.tilOther);
            lyQuestion = v.findViewById(R.id.lyQuestion);
            txtQuestion = v.findViewById(R.id.txtQuestion);
            radioYes = v.findViewById(R.id.radioYes);
            radioNo = v.findViewById(R.id.radioNo);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public CheckInputListAdapter(Context ctx, List<ObjectInputModel> items, boolean isHorizontal) {
        this.ctx = ctx;
        filtered_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
        mIsHorizontal = isHorizontal;
    }

    @Override
    public CheckInputListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate((mIsHorizontal ? R.layout.item_check_input_horizontal : R.layout.item_check_input), parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final ObjectInputModel checkBoxModel = filtered_items.get(position);
        holder.txtName.setText(checkBoxModel.Name);
        holder.cbCheck.setChecked(checkBoxModel.Check);
        if (!checkBoxModel.OtherName.isEmpty()) {
            holder.txtOther.setVisibility(View.VISIBLE);
            holder.tilOther.setVisibility(View.VISIBLE);
            holder.tilOther.setHint(checkBoxModel.OtherName);
        } else {
            holder.txtOther.setVisibility(View.GONE);
            holder.tilOther.setVisibility(View.GONE);
        }

        if (checkBoxModel.Enabled != null && checkBoxModel.Enabled) {
            holder.txtName.setTextColor(ContextCompat.getColor(this.ctx,R.color.grey_400));
        } else {
            holder.txtName.setTextColor(ContextCompat.getColor(this.ctx,  R.color.grey_1000));
        }

        if (!mIsHorizontal) {
            if (checkBoxModel.Question != null && !checkBoxModel.Question.isEmpty()) {
                holder.txtQuestion.setText(checkBoxModel.Question);
                holder.lyQuestion.setVisibility(View.VISIBLE);
                holder.radioYes.setChecked(checkBoxModel.YesValue);
                holder.radioNo.setChecked(checkBoxModel.NoValue);

                holder.radioYes.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                    @Override
                    public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                        RadioButton radioButton = (RadioButton) compoundButton;
                        if (mOnItemClickListener != null) {
                            mOnItemClickListener.onRadioCheck(position, radioButton);
                        }
                    }
                });
                // view detail message conversation
                holder.radioNo.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                    @Override
                    public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                        RadioButton radioButton = (RadioButton) compoundButton;
                        if (mOnItemClickListener != null) {
                            mOnItemClickListener.onRadioCheck(position, radioButton);
                        }
                    }
                });
            } else {
                holder.lyQuestion.setVisibility(View.GONE);
            }
        }

        // view detail message conversation
        holder.cbCheck.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(view, position, checkBoxModel);
                }
            }
        });

        holder.txtOther.setText(checkBoxModel.OtherValue);

        holder.txtOther.addTextChangedListener(new TextWatcher() {
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
