package nts.swipesafe.adapter;

import android.content.Context;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.ChildAbuseModel;
import nts.swipesafe.model.ChildModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class ChildDetailListAdapter extends RecyclerView.Adapter<ChildDetailListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ChildModel> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onEditClick(View view, int position, ChildModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout lyName, lyGender, lyBirthday, lyAge, lyAddress,lyDelete;
        public TextView txtName, txtGender, txtBirthday, txtAge, txtAddress;
        public TextView txtTitle;

        public ViewHolder(View v) {
            super(v);
            lyName = v.findViewById(R.id.lyName);
            lyGender = v.findViewById(R.id.lyGender);
            lyBirthday = v.findViewById(R.id.lyBirthday);
            lyAge = v.findViewById(R.id.lyAge);
            lyAddress = v.findViewById(R.id.lyAddress);

            txtName = v.findViewById(R.id.txtName);
            txtGender = v.findViewById(R.id.txtGender);
            txtBirthday = v.findViewById(R.id.txtBirthday);
            txtAge = v.findViewById(R.id.txtAge);
            txtAddress = v.findViewById(R.id.txtAddress);

            lyDelete = v.findViewById(R.id.lyDelete);
            txtTitle = v.findViewById(R.id.txtTitle);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public ChildDetailListAdapter(Context ctx, List<ChildModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<ChildModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public ChildDetailListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_child_detail, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final ChildModel childModel = listObject.get(position);

        holder.txtTitle.setText(holder.txtTitle.getText() + " #" + (position + 1));

        if (!Utils.isEmpty(childModel.Name)) {
            holder.lyName.setVisibility(View.VISIBLE);
            holder.txtName.setText(childModel.Name);
        } else {
            holder.lyName.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(childModel.GenderName)) {
            holder.lyGender.setVisibility(View.VISIBLE);
            holder.txtGender.setText(childModel.GenderName);
        } else {
            holder.lyGender.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(childModel.Birthday)) {
            holder.lyBirthday.setVisibility(View.VISIBLE);
            holder.txtBirthday.setText(childModel.Birthday);
        } else {
            holder.lyBirthday.setVisibility(View.GONE);
        }

        if (childModel.Age != null) {
            holder.lyAge.setVisibility(View.VISIBLE);
            holder.txtAge.setText(String.valueOf(childModel.Age));
        } else {
            holder.lyAge.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(childModel.FullAddress)) {
            holder.lyAddress.setVisibility(View.VISIBLE);
            holder.txtAddress.setText(childModel.FullAddress);
        } else {
            holder.lyAddress.setVisibility(View.GONE);
        }

        holder.lyDelete.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onEditClick(view, position, childModel);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
