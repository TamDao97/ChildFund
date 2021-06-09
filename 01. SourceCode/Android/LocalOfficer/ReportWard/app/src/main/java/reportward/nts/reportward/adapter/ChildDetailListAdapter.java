package reportward.nts.reportward.adapter;

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

import reportward.nts.reportward.R;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.ChildModel;

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
        public LinearLayout lyName, lyGender, lyBirthday, lyAge, lyLevel, lyAddress, lyDateAction;
        public TextView txtName, txtGender, txtBirthday, txtAge, txtLevel, txtAddress, txtDateAction;
        public RecyclerView rvAbuse;
        public TextView txtEdit, txtTitle;

        public ViewHolder(View v) {
            super(v);
            lyName = v.findViewById(R.id.lyName);
            lyGender = v.findViewById(R.id.lyGender);
            lyBirthday = v.findViewById(R.id.lyBirthday);
            lyAge = v.findViewById(R.id.lyAge);
            lyLevel = v.findViewById(R.id.lyLevel);
            lyAddress = v.findViewById(R.id.lyAddress);
            lyDateAction = v.findViewById(R.id.lyDateAction);

            txtName = v.findViewById(R.id.txtName);
            txtGender = v.findViewById(R.id.txtGender);
            txtBirthday = v.findViewById(R.id.txtBirthday);
            txtAge = v.findViewById(R.id.txtAge);
            txtLevel = v.findViewById(R.id.txtLevel);
            //txtAddress = v.findViewById(R.id.txtAddress);
            txtDateAction = v.findViewById(R.id.txtDateAction);

            rvAbuse = v.findViewById(R.id.rvAbuse);

            txtEdit = v.findViewById(R.id.txtEdit);
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
            holder.txtBirthday.setText(DateUtils.ConvertYMDServerToDMY(childModel.Birthday));
        } else {
            holder.lyBirthday.setVisibility(View.GONE);
        }

        if (childModel.Age != null) {
            holder.lyAge.setVisibility(View.VISIBLE);
            holder.txtAge.setText(String.valueOf(childModel.Age));
        } else {
            holder.lyAge.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(childModel.LevelName)) {
            holder.lyLevel.setVisibility(View.VISIBLE);
            holder.txtLevel.setText(childModel.LevelName);
        } else {
            holder.lyLevel.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(childModel.FullAddress)) {
            holder.lyAddress.setVisibility(View.VISIBLE);
            holder.txtAddress.setText(childModel.FullAddress);
        } else {
            holder.lyAddress.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(childModel.DateAction)) {
            holder.lyDateAction.setVisibility(View.VISIBLE);
            holder.txtDateAction.setText(DateUtils.ConvertYMDHHmmServerToDMYHHmm(childModel.DateAction));
        } else {
            holder.lyDateAction.setVisibility(View.GONE);
        }

        if (childModel.ListAbuse != null && childModel.ListAbuse.size() > 0) {
            holder.rvAbuse.setLayoutManager(new GridLayoutManager(ctx, 1));
            holder.rvAbuse.setHasFixedSize(true);
            final AbuseListAdapter abuseListAdapter = new AbuseListAdapter(ctx, childModel.ListAbuse, false);
            holder.rvAbuse.setAdapter(abuseListAdapter);
            abuseListAdapter.SetOnItemClickListener(new AbuseListAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, boolean isCheck) {
                }
            });
        }

        holder.txtEdit.setOnClickListener(new View.OnClickListener() {
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
