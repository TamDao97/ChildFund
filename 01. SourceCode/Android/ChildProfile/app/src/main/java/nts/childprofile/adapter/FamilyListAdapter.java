package nts.childprofile.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import nts.childprofile.R;
import nts.childprofile.common.Constants;
import nts.childprofile.model.FamilyMemberModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class FamilyListAdapter extends RecyclerView.Adapter<FamilyListAdapter.ViewHolder> {

    private final int mBackground;
    private List<FamilyMemberModel> original_items = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onButtonEditClick(View view, int position, FamilyMemberModel obj);

        void onButtonDeleteClick(View view, int position, FamilyMemberModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtName, txtRelationship, txtLiveTogether;
        public LinearLayout btnDelete, btnEdit;

        public ViewHolder(View v) {
            super(v);
            txtName = (TextView) v.findViewById(R.id.txtName);
            txtRelationship = (TextView) v.findViewById(R.id.txtRelationship);
            txtLiveTogether = (TextView) v.findViewById(R.id.txtLiveTogether);
            btnEdit = (LinearLayout) v.findViewById(R.id.btnEdit);
            btnDelete = (LinearLayout) v.findViewById(R.id.btnDelete);
        }
    }


    // Provide a suitable constructor (depends on the kind of dataset)
    public FamilyListAdapter(Context ctx, List<FamilyMemberModel> items) {
        this.ctx = ctx;
        original_items = items;
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public FamilyListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_family, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final FamilyMemberModel familyInfoModel = original_items.get(position);
        holder.txtName.setText(familyInfoModel.Name);
        holder.txtRelationship.setText(familyInfoModel.RelationshipName);
        holder.txtLiveTogether.setText(familyInfoModel.LiveWithChild == 1 ? "Có/Yes" : "Không/No");
        holder.btnEdit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onButtonEditClick(view, position, familyInfoModel);
                }
            }
        });

        holder.btnDelete.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onButtonDeleteClick(view, position, familyInfoModel);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return original_items.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
